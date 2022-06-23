using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class DocumentSetsController : Controller
    {
        public DocumentSetsController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
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
            ViewBag.ActiveInterfaceItem = MainMenu.DocumentSets;

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
        public ActionResult GetDocumentSetDetails(Guid? id, Guid[] selectedDocuments)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = DocumentSetsService.GetViewModel(id, selectedDocuments, IUsersHelper.CurrentUser.OrganizationID, context);
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
        public ActionResult SetDocumentSetDetails(DocumentSetModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    DocumentSetsService.SetDocumentSet(model, context);
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
                DocumentSetsService.DeleteDocumentSet(id, context);
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
            return Json(ListsHelper.DocumentSets(IUsersHelper.CurrentUser.OrganizationID).Where(ds => String.IsNullOrEmpty(term) || ds.Name.ToLower().Contains(term.ToLower())).Select(
                ds => new Select2ListItem() { id = ds.ID, text = ds.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedItems(string selection)
        {
            var result = new List<object>() { new { id = Guid.Empty, text = String.Empty } };
            result.Clear();

            foreach (var id in selection.ToGuidArray())
            {

                var documentSet = EntityContext.Current.DocumentSets.FirstOrDefault(ds => ds.ID == id);
                if (documentSet != null)
                    result.Add(new { id = documentSet.ID, text = documentSet.Name });
                else
                    result.Add(new { id = id, text = "Document Set with ID {0} does not exist anymore".ToFormat(id) });
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
                var documentSet = DocumentSetsService.AddNew(text, IUsersHelper.CurrentUser.OrganizationID, context);
                context.SaveChanges();
                id = documentSet.ID;
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
