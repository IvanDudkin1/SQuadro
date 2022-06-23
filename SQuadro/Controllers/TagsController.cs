using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        public TagsController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;
        private EntityContext context = EntityContext.Current;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.DocumentsGroup;
            ViewBag.ActiveInterfaceItem = MainMenu.DocumentTags;

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
        public ActionResult GetTagDetails(int? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = DocumentTagsService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
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
        public ActionResult SetTagDetails(TagModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    DocumentTagsService.SetTag(model, context);
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
                DocumentTagsService.DeleteTag(id, context);
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
            return Json(ListsHelper.Tags(IUsersHelper.CurrentUser.OrganizationID).Where(
                c => String.IsNullOrEmpty(term) || c.Name.ToLower().Contains(term.ToLower())).Select(
                    c => new { id = c.Name, text = c.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedItem(string selection)
        {
            var selectionList = selection.Split(',').Select(s => new { id = s, text = s }); 
            return Json(selectionList);
        }

        [HttpPost]
        public ActionResult AddNew(string text)
        {
            if (text.Length > 32)
                text = text.Substring(0, 32);
            bool result = false;
            string description = String.Empty;
            try
            {
                var tag = DocumentTagsService.AddNew(text, IUsersHelper.CurrentUser.OrganizationID, context);
                context.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, ID = text });
        }
    }
}