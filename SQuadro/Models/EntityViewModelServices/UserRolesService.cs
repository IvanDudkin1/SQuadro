using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class UserRolesService
    {
        private static void UpdateUserRoleFromModel(UserRole userRole, UserRoleModel model, EntityContext context)
        {
            userRole.OrganizationID = model.OrganizationID;
            userRole.Name = model.Name;
            userRole.IsReadonly = model.IsReadOnly;

            Guid tmpID = Guid.Empty;

            // categories
            List<Guid> categories = new List<Guid>();
            if (!String.IsNullOrWhiteSpace(model.Categories))
                categories = model.Categories.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID).ToList();

            if (userRole.Categories != null)
            {
                foreach (var companyCategory in userRole.Categories.ToList().Where(
                    cc => !categories.Any(id => id == cc.ID)))
                {
                    userRole.Categories.Remove(companyCategory);
                }
            }

            foreach (var categoryID in categories)
            {
                if (categoryID != Guid.Empty && (userRole.Categories == null || !userRole.Categories.Any(cc => cc.ID == categoryID)))
                {
                    var category = CategoriesService.GetCategory(categoryID, context);
                    userRole.Categories.Add(category);
                }
            }

            // related objects
            List<Guid> relatedObjects = new List<Guid>();
            if (!String.IsNullOrWhiteSpace(model.RelatedObjects))
                relatedObjects = model.RelatedObjects.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID).ToList();

            if (userRole.RelatedObjects != null)
            {
                foreach (var relatedObject in userRole.RelatedObjects.ToList().Where(
                    ro => !relatedObjects.Any(id => id == ro.ID)))
                {
                    userRole.RelatedObjects.Remove(relatedObject);
                }
            }

            foreach (var relatedObjectID in relatedObjects)
            {
                if (relatedObjectID != Guid.Empty && (userRole.RelatedObjects == null || !userRole.RelatedObjects.Any(ro => ro.ID == relatedObjectID)))
                {

                    var relatedObject = context.RelatedObjects.SingleOrDefault(ro => ro.ID == relatedObjectID);
                    if (relatedObjects != null)
                        userRole.RelatedObjects.Add(relatedObject);
                }
            }
        }

        public static UserRoleModel GetViewModel(Guid? userRoleID, Guid organizationID, EntityContext context)
        {
            UserRoleModel model = new UserRoleModel() { OrganizationID = organizationID };
            if (userRoleID.HasValue && userRoleID != Guid.Empty)
            {
                var userRole = GetUserRole(userRoleID.Value, context);

                model.ID = userRole.ID;
                model.OrganizationID = userRole.OrganizationID;
                model.Name = userRole.Name;
                model.IsReadOnly = userRole.IsReadonly;
                model.Categories = String.Join(",", userRole.Categories.Select(c => c.ID));
                model.RelatedObjects = String.Join(",", userRole.RelatedObjects.Select(c => c.ID));
            }
            return model;
        }

        public static UserRole SetUserRole(UserRoleModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            UserRole userRole = null;

            if (model.ID != Guid.Empty)
            {
                userRole = GetUserRole(model.ID, context);
            }
            else
            {
                userRole = new UserRole();
                context.UserRoles.AddObject(userRole);
            }

            UpdateUserRoleFromModel(userRole, model, context);
            return userRole;
        }

        public static UserRole GetUserRole(Guid id, EntityContext context)
        {
            var userRole = context.UserRoles.SingleOrDefault(t => t.ID == id);

            if (userRole == null)
                throw new InvalidOperationException("UserRole with ID = {0} does not exist anymore".ToFormat(id));
            return userRole;
        }

        public static void DeleteUserRole(Guid userRoleID, EntityContext context)
        {
            UserRole userRole = context.UserRoles.SingleOrDefault(t => t.ID == userRoleID);

            if (userRole != null)
            {
                context.UserRoles.DeleteObject(userRole);
            }
        }

    }
}