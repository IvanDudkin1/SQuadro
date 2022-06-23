using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQuadro.Models;

namespace SQuadro
{
    public partial class User
    {
        public bool IsAdmin { get { return this.Role == SystemRole.Admin.Value; } }

        public bool IsReadonly { get { return this.UserRole != null && this.UserRole.IsReadonly; } }

        public bool CanAddCategory { get { return this.UserRole == null || !this.UserRole.Categories.Any(); } }

        public bool CanAddRelatedObject { get { return this.UserRole == null || !this.UserRole.RelatedObjects.Any(); } }

        public IEnumerable<Guid> AvailableCategories
        {
            get
            {
                return this.Organization.Categories
                    .Where(c =>
                        this.UserRole == null
                        || !this.UserRole.Categories.Any()
                        || this.UserRole.Categories.Any(uc => uc.ID == c.ID))
                    .Select(c => c.ID);
            }
        }

        public IEnumerable<Guid> AvailableRelatedObjects
        {
            get
            {
                return this.Organization.RelatedObjects
                    .Where(ro =>
                        this.UserRole == null
                        || !this.UserRole.RelatedObjects.Any()
                        || this.UserRole.RelatedObjects.Any(uro => uro.ID == ro.ID))
                    .Select(ro => ro.ID);
            }
        }
    }
}