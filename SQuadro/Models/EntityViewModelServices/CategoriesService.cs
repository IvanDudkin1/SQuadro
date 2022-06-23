using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class CategoriesService
    {
        private static void UpdateCategoryFromModel(Category category, CategoryModel model, User currentUser, EntityContext context)
        {
            if (!currentUser.CanAddCategory && !currentUser.AvailableCategories.Contains(category.ID))
                throw new UserException("Modifying is forbidden");

            category.OrganizationID = model.OrganizationID;
            category.Name = model.Name;
        }

        public static Category GetCategory(Guid categoryID, EntityContext context)
        {
            var category = context.Categories.SingleOrDefault(a => a.ID == categoryID);
            if (category == null)
                throw new InvalidOperationException("Category with ID = {0} does not exist anymore".ToFormat(categoryID));
            return category;
        }

        public static CategoryModel GetViewModel(Guid? categoryID, Guid organizationID, EntityContext context)
        {
            CategoryModel model = new CategoryModel() { OrganizationID = organizationID };
            if (categoryID != null && categoryID != Guid.Empty)
            {
                var category = GetCategory(categoryID.Value, context);

                model.ID = category.ID;
                model.OrganizationID = category.OrganizationID;
                model.Name = category.Name;
            }
            return model;
        }

        public static Category SetCategory(CategoryModel model, User currentUser, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Category category = null;

            if (model.ID != Guid.Empty)
            {
                category = GetCategory(model.ID, context);
            }
            else
            {
                category = new Category();
                context.Categories.AddObject(category);
            }

            UpdateCategoryFromModel(category, model, currentUser, context);
            return category;
        }

        public static void DeleteCategory(Guid categoryID, User currentUser, EntityContext context)
        {
            if (!currentUser.CanAddCategory)
                throw new UserException("Deletion is forbidden");

            Category category = context.Categories.SingleOrDefault(c => c.ID == categoryID);
            if (category.CategoryCompanies.Any())
                throw new InvalidOperationException("There are Partners with this Category. Deletion aborted");

            if (category != null)
            {
                context.Categories.DeleteObject(category);
            }
        }

        public static Category AddNew(string name, User currentUser, EntityContext context)
        {
            if (!currentUser.CanAddCategory)
                throw new UserException("Adding new categories is forbidden.");

            Category category = new Category() { OrganizationID = currentUser.OrganizationID, Name = name };
            context.Categories.AddObject(category);
            return category;
        }


    }
}