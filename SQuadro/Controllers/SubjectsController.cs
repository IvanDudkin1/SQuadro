using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        public SubjectsController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;
        private EntityContext context = EntityContext.Current;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.EmailSettings;
            ViewBag.ActiveInterfaceItem = MainMenu.Subjects;

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
        public ActionResult GetSubjectDetails(int? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = SubjectsService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
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
        public ActionResult SetSubjectDetails(SubjectModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    SubjectsService.SetSubject(model, context);
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
                SubjectsService.DeleteSubject(id, context);
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
            return Json(ListsHelper.Subjects(IUsersHelper.CurrentUser.OrganizationID).Where(
                c => String.IsNullOrEmpty(term) || c.Text.ToLower().Contains(term.ToLower())).Select(
                    c => new { id = c.Text, text = c.Text }));
        }

        [HttpPost]
        public ActionResult GetSelectedItem(string selection)
        {
            return Json(new { id = selection, text = selection });
        }

        [HttpPost]
        public ActionResult AddNew(string text)
        {
            bool result = false;
            string description = String.Empty;
            try
            {
                var subject = SubjectsService.AddNew(text, IUsersHelper.CurrentUser.OrganizationID, context);
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