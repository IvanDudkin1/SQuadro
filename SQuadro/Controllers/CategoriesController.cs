using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        public CategoriesController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
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
            ViewBag.ActiveInterfaceItem = MainMenu.Categories;

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
        public ActionResult GetCategoryDetails(Guid? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = CategoriesService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
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
        public ActionResult SetCategoryDetails(CategoryModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    CategoriesService.SetCategory(model, IUsersHelper.CurrentUser, context);
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
                CategoriesService.DeleteCategory(id, IUsersHelper.CurrentUser, context);
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
            return Json(ListsHelper.PartnerCategories(IUsersHelper.CurrentUser)
                .Where(pc => String.IsNullOrEmpty(term) || pc.Name.ToLower().Contains(term.ToLower()))
                .Select(pc => new Select2ListItem() { id = pc.ID, text = pc.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedItems(string selection)
        {
            var result = new List<object>() { new { id = Guid.Empty, text = String.Empty } };
            result.Clear();

            Guid tmpID = Guid.Empty;

            foreach (var id in selection.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID))
            {
                Category category = EntityContext.Current.Categories.FirstOrDefault(c => c.ID == id);
                if (category != null)
                    result.Add(new { id = category.ID, text = category.Name });
                else
                    result.Add(new { id = id, text = "Category with ID {0} does not exist anymore".ToFormat(id) });
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
                var category = CategoriesService.AddNew(text, IUsersHelper.CurrentUser, context);
                context.SaveChanges();
                id = category.ID;
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
