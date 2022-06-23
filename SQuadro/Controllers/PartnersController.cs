using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class PartnersController : Controller
    {
        public PartnersController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;
        private EntityContext context = EntityContext.Current;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.CompaniesGroup;
            ViewBag.ActiveInterfaceItem = MainMenu.Companies;

            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.Initialize();
            return PartialView("ListTemplateView", IListTemplate);
        }

        public ActionResult IndexPartial()
        {
            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.InitParams = Tuple.Create<object, object, object>(true, null, null);
            IListTemplate.Initialize();

            return PartialView("ListTemplateViewPartial", IListTemplate);
        }

        [HttpPost]
        public ActionResult DataSourceCallback(DataTablesParam param)
        {
            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.Initialize();
            int totalRecords, filteredRecords;
            var result = IListTemplate.GetDataSource(param, Request, out totalRecords, out filteredRecords);

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
            });
        }

        [HttpPost]
        public ActionResult GetPartnerDetails(Guid? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = PartnersService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
                content = this.PartialViewToString("Details", model);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult SetPartnerDetails(PartnerModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    PartnersService.SetPartner(model, IUsersHelper.CurrentUser, context);
                    context.SaveChanges();
                    result = true;
                }
                else
                {
                    ModelState.AddModelError("", "Please correct all errors.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetMessage());
            }
            string content = String.Empty;
            if (!result) content = this.PartialViewToString("Details", model);
            return Json(new { Result = result, Content = content });
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            bool result = false;
            string description = String.Empty;
            try
            {
                PartnersService.DeletePartner(id, context);
                context.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description });
        }

        [HttpPost]
        public ActionResult DeleteContact(PartnerModel model, int index)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;
            try
            {
                ModelState.Clear();
                if (0 <= index && index < model.Contacts.Count)
                    model.Contacts.RemoveAt(index);
                content = this.PartialViewToString("Details", model);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult Email(Guid[] partners, Guid[] documents)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;
            try
            {
                string[] recipients = PartnersService.GetRecipients(partners, context);
                SendDocumentsModel model = new SendDocumentsModel() { DocumentsSelection = String.Join(",", documents), RecipientsSelection = String.Join(",", recipients), SendAsanAttachment = true };
                var senderTemplate = ListsHelper.EmailSettings(IUsersHelper.CurrentUser.OrganizationID).FirstOrDefault();
                if (senderTemplate != null)
                    model.Sender = senderTemplate.ID;
                content = this.PartialViewToString("SendDocuments", model);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult SendDocuments(SendDocumentsModel model)
        {
            bool result = false;
            string content = String.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = IUsersHelper.CurrentUser;
                    List<Document> documents = new List<Document>();
                    Guid tmpID = Guid.Empty;
                    foreach (var item in model.DocumentsSelection.Split(','))
                    {
                        if (Guid.TryParse(item, out tmpID))
                        {
                            var document = context.Documents.FirstOrDefault(d => d.ID == tmpID);
                            if (document != null)
                                documents.Add(document);
                        }
                    }

                    string[] recipients = model.RecipientsSelection.Split(',');

                    EmailDefinition emailDefinition;

                    if (model.SendAsanAttachment)
                    {
                        emailDefinition = new EmailDefinition(currentUser.OrganizationID, EmailTemplate.SendDocumentsAsAttachments, new Dictionary<string, string>()
                            {
                                { "sender_user_name", currentUser.Name },
                                { "sender_organization_name", currentUser.Organization.Name }
                            });
                        emailDefinition.AddAttachments(documents
                            .Select(d => new CustomAttachment(d.FileName, d.File))
                            .ToArray());
                    }
                    else
                    {
                        DateTime expirationDate = DateTime.Today.AddDays(20);
                        string token = SecurityHelper.GetDateTimeHash(expirationDate);
                        string links = String.Join(String.Empty, documents.Select(
                            d => @"<p><a href={0}>{1}</a></p>".ToFormat(MvcHelper.GetFullUrl("Download", "Documents", new { id = d.ID, expiration = expirationDate.Ticks, token = token }), d.Name)));

                        emailDefinition = new EmailDefinition(currentUser.OrganizationID, EmailTemplate.SendDocumentsAsLinks, new Dictionary<string, string>()
                            {
                                { "sender_user_name", currentUser.Name },
                                { "sender_organization_name", currentUser.Organization.Name },
                                { "document_links", links }
                            });
                    }

                    emailDefinition.Subject = model.Subject;

                    if (model.Sender.HasValue)
                    {
                        var senderModel = EmailSettingsService.GetViewModel(model.Sender, currentUser.OrganizationID, context);
                        if (senderModel != null)
                        {
                            emailDefinition.EmailFrom = new EmailFrom(senderModel.Name, senderModel.Email);
                            emailDefinition.SmtpServer = senderModel.SmtpServer;
                            emailDefinition.SmtpPort = senderModel.SmtpPort;
                            emailDefinition.SmtpUser = senderModel.SmtpUser;
                            emailDefinition.SmtpPassword = senderModel.SmtpPassword;
                            emailDefinition.EnableSsl = senderModel.EnableSsl;
                        }
                    }
                    else
                    {
                        emailDefinition.EmailFrom = new EmailFrom(currentUser.Organization.Name, emailDefinition.SmtpUser);
                    }

                    emailDefinition.AddRecipients(recipients);    
                    EmailSender.SendEmail(emailDefinition);
                    result = true;
                }
                else
                {
                    ModelState.AddModelError("", "Please correct all errors.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetMessage());
            }

            if (!result) content = this.PartialViewToString("SendDocuments", model);
            return Json(new { Result = result, Content = content });
        }

        [HttpPost]
        public ActionResult AddNewContact(PartnerModel model)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                ModelState.Clear();
                if (model.Contacts == null)
                    model.Contacts = new List<ContactModel>();
                model.Contacts.Insert(0, new ContactModel());
                content = this.PartialViewToString("Details", model);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult GetContacts(Guid id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var partnerModel = PartnersService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
                content = this.PartialViewToString("ContactsPartial", partnerModel.Contacts);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult GetRecipientsList(string term)
        {
            var result = Json(ListsHelper.Recipients(IUsersHelper.CurrentUser.OrganizationID).Where(
                c => String.IsNullOrEmpty(term) || c.Data.ToLower().Contains(term.ToLower()) || c.Company.Name.ToLower().Contains(term.ToLower())).Select(
                    c => new { id = c.Data, text = String.Concat(c.Company.Name, " (", c.Data, ")"), fake = "" }));
            return result;
        }

        [HttpPost]
        public ActionResult GetSelectedRecipients(string selection)
        {
            var selectionArr = selection.Split(',');
            
            var result = context.Contacts.Where(c => c.Company.OrganizationID == IUsersHelper.CurrentUser.OrganizationID && selectionArr.Contains(c.Data)).OrderBy(
                c => c.Company.Name).Select(c => new { id = c.Data, text = c.Company.Name + " (" + c.Data + ")" });

            return Json(result);
        }
    }
}
