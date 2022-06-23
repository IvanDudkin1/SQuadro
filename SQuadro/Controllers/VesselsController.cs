using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class VesselsController : Controller
    {
        public VesselsController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;
        private EntityContext context = EntityContext.Current;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.RelatedObjectsGroup;
            ViewBag.ActiveInterfaceItem = MainMenu.Vessels;

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
        public ActionResult GetVesselDetails(Guid? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = VesselsService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
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
        public ActionResult SetVesselDetails(VesselModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    VesselsService.SetVessel(model, IUsersHelper.CurrentUser, context);
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
                VesselsService.DeleteVessel(id, IUsersHelper.CurrentUser, context);
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
            return Json(ListsHelper.Vessels(IUsersHelper.CurrentUser).Where(c => String.IsNullOrEmpty(term) || c.Name.ToLower().Contains(term.ToLower())).Select(
                c => new { id = c.ID, text = c.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedItem(Guid selection)
        {
            string result = String.Empty;
            var relatedObject = EntityContext.Current.RelatedObjects.FirstOrDefault(c => c.ID == selection);
            if (relatedObject != null)
                result = relatedObject.Name;
            else
                result = "Related Object with ID {0} does not exist anymore".ToFormat(selection);

            return Json(new { id = selection, text = result });
        }

        [HttpPost]
        public ActionResult GetSelectedItems(string selection)
        {
            var result = new List<object>() { new { id = Guid.Empty, text = String.Empty } };
            result.Clear();

            Guid tmpID = Guid.Empty;

            foreach (var id in selection.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID))
            {
                RelatedObject relatedObject = EntityContext.Current.RelatedObjects.SingleOrDefault(ro => ro.ID == id);
                if (relatedObject != null)
                    result.Add(new { id = relatedObject.ID, text = relatedObject.Name });
                else
                    result.Add(new { id = id, text = "Related Object with ID {0} does not exist anymore".ToFormat(id) });
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddNew(string text)
        {
            bool result = false;
            string description = String.Empty;
            Guid? id = null;
            try
            {
                var vessel = VesselsService.AddNew(text, IUsersHelper.CurrentUser, context);
                EntityContext.Current.SaveChanges();
                id = vessel.ID;
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
