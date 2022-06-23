using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Filters;
using SQuadro.Models;
using System.Web.Security;
using System.Text.RegularExpressions;
using WebMatrix.WebData;

namespace SQuadro.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    [EnforcePasswordPolicy]
    [HandleError(View = "Error")]
    public class UsersController : Controller
    {
        public UsersController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;
        private EntityContext context = EntityContext.Current;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.AdministrationGroup;
            ViewBag.ActiveInterfaceItem = MainMenu.Users;

            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.Initialize();
            return PartialView("ListTemplateView", IListTemplate);
        }

        [HttpPost]
        public ActionResult DataSourceCallback(DataTablesParam param, Guid parentID)
        {
            IListTemplate.ParentID = parentID;
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
        public ActionResult GetUserDetails(Guid? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = UsersService.GetViewModel(id, context);
                content = this.PartialViewToString("Details", model);
                result = true;
            }
            catch (Exception e)
            {
                description = ExceptionsHelper.ProcessException(e);
            }

            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult SetUserDetails(UserModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    bool IsNewUser = (model.ID == Guid.Empty);
                    var user = UsersService.SetUser(model, IUsersHelper.CurrentUser, context);
                    //using (var transactionScope = new TransactionScope())
                    {
                        context.SaveChanges();

                        if (IsNewUser)
                        {
                            var currentUser = IUsersHelper.CurrentUser;
                            string password = Membership.GeneratePassword(6, 0);
                            password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "7");
                            WebSecurity.CreateUserAndAccount(user.ID.ToString(), password);

                            EmailDefinition emailDefinition = new EmailDefinition(currentUser.OrganizationID, EmailTemplate.InviteUser, new Dictionary<string, string>()
                                {
                                    { "recipient_user_name", user.Name },
                                    { "recipient_organization_name", user.Organization.Name },
                                    { "password", password },
                                    { "sender_user_name", currentUser.Name },
                                    { "sender_organization_name", currentUser.Organization.Name },
                                    { "app_link", MvcHelper.GetFullUrl("Index", "Home") }
                                });
                            emailDefinition.EmailFrom = new EmailFrom(currentUser.Organization.Name, emailDefinition.SmtpUser);
                            emailDefinition.AddRecipients(new string[] { user.Email });    
                            EmailSender.SendEmail(emailDefinition);
                        }
                        //transactionScope.Complete();
                    }
                    result = true;
                }
                else
                {
                    ModelState.AddModelError("", "Please correct all errors.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ExceptionsHelper.ProcessException(e));
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
            bool forceLogOut = false;
            try
            {
                //using (var transactionScope = new TransactionScope())
                {
                    bool forceDeleteUser;
                    var user = UsersService.DeleteUser(id, context);
                    context.SaveChanges();
                    Membership.DeleteUser(user.ID.ToString());
                    if (user.ID == IUsersHelper.CurrentUser.ID)
                    {
                        WebSecurity.Logout();
                        forceLogOut = true;
                    }
                    //transactionScope.Complete();
                }
                result = true;
            }
            catch (Exception e)
            {
                description = ExceptionsHelper.ProcessException(e);
            }
            return Json(new { Result = result, Description = description, ForceLogOut = forceLogOut });
        }
    }
}
