using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace SQuadro.Models
{
    public static class ListsHelper
    {
        public static IEnumerable<Category> PartnerCategories(User currentUser)
        {
            return EntityContext.Current.Categories
                .Where(c => c.OrganizationID == currentUser.OrganizationID && currentUser.AvailableCategories.Contains(c.ID))
                .OrderBy(c => c.Name);
        }

        public static IEnumerable<SelectListItem> PartnerCategoriesList(User currentUser, bool addAll = false)
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            if (addAll)
                categories.Add(new SelectListItem() { Value = string.Empty, Text = "All Categories" } );

            categories.AddRange(PartnerCategories(currentUser)
                .ToList()
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name }));

            return categories;
        }

        public static IEnumerable<Area> PartnerAreas(Guid organizationID)
        {
            return EntityContext.Current.Areas.Where(a => a.OrganizationID == organizationID).OrderBy(a => a.Name);
        }

        public static IEnumerable<SelectListItem> PartnerAreasList(Guid organizationID, bool addAll = false)
        {
            List<SelectListItem> areas = new List<SelectListItem>();
            if (addAll)
                areas.Add(new SelectListItem() { Value = string.Empty, Text = "All Areas" });

            areas.AddRange(PartnerAreas(organizationID).ToList().Select(
                a => new SelectListItem() { Value = a.ID.ToString(), Text = a.Name }));

            return areas;
        }

        public static IEnumerable<Country> Countries()
        {
            return EntityContext.Current.Countries.OrderBy(c => c.Name);
        }

        public static IEnumerable<SelectListItem> CountriesList(bool addAll = false)
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            if (addAll)
                countries.Add(new SelectListItem() { Value = String.Empty, Text = "All Countries" });

            countries.AddRange(Countries().ToList().Select(
                c => new SelectListItem() { Value = c.ID_Alpha2.ToString(), Text = c.Name }));

            return countries;
        }

        public static IEnumerable<ContactType> ContactTypes(Guid organizationID)
        {
            return EntityContext.Current.ContactTypes.Where(c => c.OrganizationID == organizationID).OrderBy(c => c.ID);
        }

        public static IEnumerable<SelectListItem> ContactTypesList(Guid organizationID)
        {
            var types = new List<SelectListItem>() { new SelectListItem() { Value = String.Empty, Text = String.Empty } };
            types.AddRange(ContactTypes(organizationID).ToList().Select(
                c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name }));

            return types;
        }

        public static IQueryable<DocumentType> DocumentTypes(Guid organizationID)
        {
            return EntityContext.Current.DocumentTypes.Where(dt => dt.OrganizationID == organizationID).OrderBy(dt => dt.Name);
        }

        public static IEnumerable<SelectListItem> DocumentTypesList(Guid organizationID)
        {
            var types = new List<SelectListItem>() { new SelectListItem() { Value = String.Empty, Text = "All Types" } };
            types.AddRange(DocumentTypes(organizationID).ToList().Select(
                c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name }));

            return types;
        }

        public static IQueryable<DocumentStatus> DocumentStatuses(Guid organizationID)
        {
            return EntityContext.Current.DocumentStatuses.Where(ds => ds.OrganizationID == organizationID).OrderBy(ds => ds.Name);
        }

        public static IEnumerable<SelectListItem> DocumentStatusesList(Guid organizationID)
        {
            var statuses = new List<SelectListItem>() { new SelectListItem() { Value = String.Empty, Text = "All Statuses"  } };
            statuses.AddRange(DocumentStatuses(organizationID).ToList().Select(
                c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name }));

            return statuses;
        }

        public static IQueryable<Tag> Tags(Guid organizationID)
        {
            return EntityContext.Current.Tags.Where(t => t.OrganizationID == organizationID).OrderBy(t => t.Name);
        }

        public static IQueryable<RelatedObject> RelatedObjects(User currentUser)
        {
            return EntityContext.Current.RelatedObjects
                .Where(ro => ro.OrganizationID == currentUser.OrganizationID && currentUser.AvailableRelatedObjects.Contains(ro.ID))
                .OrderBy(v => v.Name);
        }

        public static IEnumerable<SelectListItem> RelatedObjectsList(User currentUser)
        {
            var relatedObjects = new List<SelectListItem>() { new SelectListItem() { Value = Guid.Empty.ToString(), Text = "All Objects" } };
            relatedObjects.AddRange(RelatedObjects(currentUser)
                .ToList()
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name }));

            return relatedObjects;
        }

        public static IQueryable<Vessel> Vessels(User currentUser)
        {
            return RelatedObjects(currentUser).OfType<Vessel>();
        }

        public static IEnumerable<SelectListItem> VesselsList(Guid organizationID, User currentUser)
        {
            var vessels = new List<SelectListItem>() { new SelectListItem() { Value = Guid.Empty.ToString(), Text = "All Vessels" } };
            vessels.AddRange(Vessels(currentUser)
                .ToList()
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name }));

            return vessels;
        }

        public static IEnumerable<DocumentSet> DocumentSets(Guid organizationID)
        {
            return EntityContext.Current.DocumentSets.Where(ds => ds.OrganizationID == organizationID).OrderBy(ds => ds.Name);
        }

        public static IEnumerable<SelectListItem> DocumentSetsList(Guid organizationID)
        {
            var sets = new List<SelectListItem>() { new SelectListItem() { Value = String.Empty, Text = "All Sets" } };
            sets.AddRange(DocumentSets(organizationID).ToList().Select(
                c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name }));

            return sets;
        }

        public static IQueryable<Document> Documents(Guid organizationID)
        {
            return EntityContext.Current.Documents.Where(d => d.OrganizationID == organizationID).OrderBy(dt => dt.Name);
        }

        public static IQueryable<Contact> Recipients(Guid organizationID)
        {
            return EntityContext.Current.Companies.Where(c => c.OrganizationID == organizationID).SelectMany(
                c => c.Contacts.Where(cnt => cnt.ContactType.SystemType == SystemContactType.Email.Value)).OrderBy(
                    c => c.Company.Name).ThenBy(c => c.Data);
        }

        public static IEnumerable<SelectListItem> SystemContactTypesList()
        {
            List<SelectListItem> result = new List<SelectListItem>() { new SelectListItem() { Value = null, Text = String.Empty } };
            result.AddRange(SystemContactType.SystemContactTypes().OrderBy(ct => ct.Value).Select(
                ct => new SelectListItem() { Value = ct.Value.ToString(), Text = ct.Text }));
            return result;
        }

        public static IEnumerable<EmailSettings> EmailSettings(Guid organizationID)
        {
            return EntityContext.Current.EmailSettings.Where(e => e.OrganizationID == organizationID).OrderByDescending(
                e => e.IsDefault).ThenBy(e => e.Name);
        }

        public static IEnumerable<SelectListItem> EmailSettingsList(Guid organizationID)
        {
            return EmailSettings(organizationID).Select(
                e => new SelectListItem() { Value = e.ID.ToString(), Text = "{0} ({1})".ToFormat(e.Name, e.Email) });
        }

        public static IEnumerable<SelectListItem> EmailTemplatesList()
        {
            List<SelectListItem> result = new List<SelectListItem>() { new SelectListItem() { Value = null, Text = String.Empty } };
            result.AddRange(EmailTemplate.EmailTemplates()
                .OrderBy(et => et.Value)
                .Select(et => new SelectListItem() { Value = et.Value.ToString(), Text = et.Text }));
            return result;
        }

        public static IEnumerable<SelectListItem> SystemRolesList()
        {
            return SystemRole.SystemRoles().Select(
                ur => new SelectListItem() { Value = ur.Value.ToString(), Text = ur.Text });
        }

        public static IEnumerable<SelectListItem> UserRolesList(Guid organizationID)
        {
            var roles = new List<SelectListItem>() { new SelectListItem() { Value = String.Empty, Text = "" } };
            roles.AddRange(EntityContext.Current.UserRoles
                .Where(ur => ur.OrganizationID == organizationID)
                .AsEnumerable()
                .Select(ur => new SelectListItem() { Value = ur.ID.ToString(), Text = ur.Name }));
            return roles;
        }

        public static IEnumerable<SelectListItem> TimeZonesList()
        {
            var result = TimeZoneInfo.GetSystemTimeZones().ToList().Select(tz => new SelectListItem() { Value = tz.Id, Text = tz.DisplayName });
            return result;
        }

        public static IQueryable<Subject> Subjects(Guid organizationID)
        {
            return EntityContext.Current.Subjects.Where(t => t.OrganizationID == organizationID).OrderBy(t => t.Text);
        }

        internal static object UserRoles(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}