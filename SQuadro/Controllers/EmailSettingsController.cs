using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class EmailSettingsController : Controller
    {
        public EmailSettingsController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;
        private EntityContext context = EntityContext.Current;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.SettingsGroup;
            ViewBag.ActiveInterfaceItem = MainMenu.EmailSettings;

            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.Initialize();
            return PartialView("ListTemplateView", IListTemplate);
        }

        [HttpPost]
        public ActionResult DataSourceCallback(DataTablesParam param, Guid parentID)
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
        public ActionResult GetEmailSettingsDetails(Guid? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = EmailSettingsService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
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
        [ValidateInput(false)]
        public ActionResult SetEmailSettingsDetails(EmailSettingsModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    EmailSettingsService.SetEmailSettings(model, context);
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
                EmailSettingsService.DeleteEmailSettings(id, context);
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
        public ActionResult GetList(string term)
        {
            return Json(ListsHelper.EmailSettings(IUsersHelper.CurrentUser.OrganizationID).Where(
                e => String.IsNullOrEmpty(term) || e.Name.ToLower().Contains(term.ToLower()) || e.Email.ToLower().Contains(term.ToLower())).Select(
                    e => new Select2ListItem() { id = e.ID.ToString(), text = "{0} ({1})".ToFormat(e.Name, e.Email) }));
        }

        [HttpPost]
        public ActionResult GetSelectedItems(string selection)
        {
            var result = new List<object>() { new { id = Guid.Empty, text = String.Empty } };
            result.Clear();

            Guid tmpID = Guid.Empty;

            foreach (var id in selection.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID))
            {

                EmailSettings emailSettings = EntityContext.Current.EmailSettings.SingleOrDefault(e => e.ID == id);
                if (emailSettings != null)
                    result.Add(new { id = emailSettings.ID.ToString(), text = "{0} ({1})".ToFormat(emailSettings.Name, emailSettings.Email) });
                else
                    result.Add(new { id = id.ToString(), text = "EmailSettings with ID {0} does not exist anymore".ToFormat(id) });
            }
            return Json(result);
        }
    }
}
