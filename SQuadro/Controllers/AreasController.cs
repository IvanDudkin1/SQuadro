using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class AreasController : Controller
    {
        public AreasController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private EntityContext context = EntityContext.Current;
        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.CompaniesGroup;
            ViewBag.ActiveInterfaceItem = MainMenu.Areas;

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
        public ActionResult GetAreaDetails(Guid? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = AreasService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
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
        public ActionResult SetAreaDetails(AreaModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    AreasService.SetArea(model, context);
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
                AreasService.DeleteArea(id, context);
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
            return Json(ListsHelper.PartnerAreas(IUsersHelper.CurrentUser.OrganizationID).Where(pa => String.IsNullOrEmpty(term) || pa.Name.ToLower().Contains(term.ToLower())).Select(
                pa => new Select2ListItem() { id = pa.ID, text = pa.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedItems(string selection)
        {
            var result = new List<object>() { new { id = Guid.Empty, text = String.Empty } };
            result.Clear();

            Guid tmpID = Guid.Empty;

            foreach (var id in selection.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID))
            {

                Area area = EntityContext.Current.Areas.FirstOrDefault(a => a.ID == id);
                if (area != null)
                    result.Add(new { id = area.ID, text = area.Name });
                else
                    result.Add(new { id = id, text = "Area with ID {0} does not exist anymore".ToFormat(id) });
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddNew(string text)
        {
            bool result = false;
            string description = String.Empty;
            Guid id = Guid.Empty;
            try
            {
                var area = AreasService.AddNew(text, IUsersHelper.CurrentUser.OrganizationID, context);
                context.SaveChanges();
                id = area.ID;
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
