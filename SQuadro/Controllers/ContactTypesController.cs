using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class ContactTypesController : Controller
    {
        public ContactTypesController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
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
            ViewBag.ActiveInterfaceItem = MainMenu.ContactTypes;

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
        public ActionResult GetContactTypeDetails(int? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = ContactTypesService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
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
        public ActionResult SetContactTypeDetails(ContactTypeModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    ContactTypesService.SetContactType(model, context);
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
        public ActionResult Delete(int id)
        {
            bool result = false;
            string description = String.Empty;
            try
            {
                ContactTypesService.DeleteContactType(id, context);
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
            return Json(ListsHelper.ContactTypes(IUsersHelper.CurrentUser.OrganizationID).Where(c => String.IsNullOrEmpty(term) || c.Name.ToLower().Contains(term.ToLower())).Select(
                c => new { id = c.ID, text = c.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedItem(int selection)
        {
            string result = String.Empty;
            ContactType contactType = context.ContactTypes.FirstOrDefault(c => c.ID == selection);
            if (contactType != null)
                result = contactType.Name;
            else
                result = "Contact type with ID {0} does not exist anymore".ToFormat(selection);

            return Json(new { id = selection, text = result });
        }

        [HttpPost]
        public ActionResult AddNew(string text)
        {
            bool result = false;
            string description = String.Empty;
            Int32? id = null;
            try
            {
                var contactType = ContactTypesService.AddNew(text, IUsersHelper.CurrentUser.OrganizationID, context);
                EntityContext.Current.SaveChanges();
                id = contactType.ID;
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, ID = id });
        }
    }
}
