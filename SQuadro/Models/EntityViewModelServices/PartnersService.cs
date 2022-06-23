using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class PartnersService
    {
        private static void UpdatePartnerFromModel(Company company, User currentUser, PartnerModel model, EntityContext context)
        {
            company.OrganizationID = model.OrganizationID;
            company.Name = model.Name;
            company.FullName = model.FullName;
            company.CountryID_Alpha2 = model.Country;
            company.Address = model.Address;
            company.Comment = model.Comment;

            if (model.ID == Guid.Empty)
            {
                company.CreatedByUserID = currentUser.ID;
                company.CreatedOn = DateTimeOffset.Now;
            }
            else
            {
                company.UpdatedByUserID = currentUser.ID;
                company.UpdatedOn = DateTimeOffset.Now;
            }

            Guid tmpID = Guid.Empty;

            // areas
            List<Guid> areas = new List<Guid>();
            if (!String.IsNullOrWhiteSpace(model.Areas))
            {
                areas = model.Areas.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID).ToList();
            }

            if (company.CompanyAreas != null)
            {
                foreach (var companyArea in company.CompanyAreas.ToList().Where(
                    ca => !areas.Any(id => id == ca.AreaID)))
                {
                    context.CompanyAreas.DeleteObject(companyArea);
                }
            }

            foreach (var areaID in areas)
            {
                if (areaID != Guid.Empty && (company.CompanyAreas == null || !company.CompanyAreas.Any(ca => ca.AreaID == areaID)))
                    context.CompanyAreas.AddObject(new CompanyArea() { Company = company, AreaID = areaID });
            }

            // categories
            List<Guid> categories = new List<Guid>();
            if (!String.IsNullOrWhiteSpace(model.Categories))
                categories = model.Categories.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID).ToList();

            if (company.CompanyCategories != null)
            {
                foreach (var companyCategory in company.CompanyCategories.ToList().Where(
                    cc => !categories.Any(id => id == cc.CategoryID)))
                {
                    context.CompanyCategories.DeleteObject(companyCategory);
                }
            }

            foreach (var categoryID in categories)
            {
                if (categoryID != Guid.Empty && (company.CompanyCategories == null || !company.CompanyCategories.Any(cc => cc.CategoryID == categoryID)))
                    context.CompanyCategories.AddObject(new CompanyCategory() { Company = company, CategoryID = categoryID });
            }

            // contacts
            if (company.Contacts != null)
            {
                foreach (var contact in company.Contacts.ToList().Where(
                    c => model.Contacts == null || !model.Contacts.Any(mc => mc.ID == c.ID)))
                {
                    context.Contacts.DeleteObject(contact);
                }
            }

            if (model.Contacts != null)
            {
                foreach (var contactModel in model.Contacts)
                {
                    Contact contact = null;
                    if (contactModel.ID != Guid.Empty)
                        contact = company.Contacts.FirstOrDefault(c => c.ID == contactModel.ID);
                    if (contact == null)
                    {
                        contact = new Contact() { Company = company };
                        context.Contacts.AddObject(contact);
                    }
                    contact.ContactTypeID = contactModel.Type.Value;
                    contact.Data = contactModel.Data;
                    contact.IsPrimary = contactModel.IsPrimary;
                    contact.Comment = contactModel.Comment;
                }
            }
        }

        public static PartnerModel GetViewModel(Guid? companyID, Guid organizationID, EntityContext context)
        {
            PartnerModel model = new PartnerModel() { OrganizationID = organizationID };
            if (companyID != null && companyID != Guid.Empty)
            {
                var company = context.Companies.FirstOrDefault(c => c.ID == companyID);
                if (company == null)
                    throw new InvalidOperationException("Company with ID = {0} does not exist anymore".ToFormat(companyID));

                model.ID = company.ID;
                model.OrganizationID = company.OrganizationID;
                model.Name = company.Name;
                model.FullName = company.FullName;
                model.Address = company.Address;
                model.Country = company.CountryID_Alpha2;
                model.Comment = company.Comment;
                model.CreatedBy = company.UserCreatedBy.Name;
                model.CreatedOn = company.CreatedOn.ToTimeZone(company.Organization.Timezone);
                model.UpdatedBy = company.UpdatedByUserID.HasValue ? company.UserUpdatedBy.Name : String.Empty;
                model.UpdatedOn = (company.UpdatedOn.HasValue ? company.UpdatedOn.Value.ToTimeZone(company.Organization.Timezone) : (DateTimeOffset?)null);

                model.Categories = String.Join(",", company.CompanyCategories.Select(cc => cc.CategoryID));
                model.Areas = String.Join(",", company.CompanyAreas.Select(ca => ca.AreaID));

                model.Contacts = company.Contacts.OrderByDescending(c => c.IsPrimary).ThenBy(c => c.ContactTypeID).Select(
                    c => new ContactModel() { 
                        ID = c.ID, 
                        CompanyID = c.CompanyID, 
                        Type = c.ContactTypeID,
                        TypeName = c.ContactType.Name,
                        ContactDisplay = String.IsNullOrWhiteSpace(c.ContactType.DisplayPattern) ? c.Data : c.ContactType.DisplayPattern.ToFormat(c.Data),
                        Data = c.Data, 
                        Comment = c.Comment, 
                        IsPrimary = c.IsPrimary 
                    }).ToList();

            }
            return model;
        }

        public static Company SetPartner(PartnerModel model, User currentUser, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("partnerModel");

            Company company = null;

            if (model.ID != Guid.Empty)
            {
                company = context.Companies.FirstOrDefault(c => c.ID == model.ID);

                if (company == null)
                    throw new InvalidOperationException("Company with ID = {0} does not exist anymore".ToFormat(model.ID));
            }
            else
            {
                company = new Company();
                context.Companies.AddObject(company);
            }

            UpdatePartnerFromModel(company, currentUser, model, context);
            return company;
        }

        public static void DeletePartner(Guid companyID, EntityContext context)
        {
            Company company = context.Companies.FirstOrDefault(c => c.ID == companyID);
            if (company != null)
            {
                foreach (var contact in company.Contacts.ToList())
                    context.Contacts.DeleteObject(contact);
                foreach (var category in company.CompanyCategories.ToList())
                    context.CompanyCategories.DeleteObject(category);
                foreach (var area in company.CompanyAreas.ToList())
                    context.CompanyAreas.DeleteObject(area);
                context.Companies.DeleteObject(company);
            }
        }

        internal static string[] GetRecipients(IEnumerable<Guid> partners, EntityContext context)
        {
            return context.Companies.Where(c => partners.Contains(c.ID)).SelectMany(
                c => c.Contacts.Where(cn => cn.ContactType.SystemType == SystemContactType.Email.Value && cn.IsPrimary)).Select(
                    i => i.Data).ToArray();
        }
    }
}