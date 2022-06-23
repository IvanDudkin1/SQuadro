using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class ContactTypesService
    {
        private static void UpdateContactTypeFromModel(ContactType contactType, ContactTypeModel model, EntityContext context)
        {
            contactType.OrganizationID = model.OrganizationID;
            contactType.Name = model.Name;
            contactType.DisplayPattern = model.DisplayPattern;
            contactType.SystemType = (model.SystemType != null ? (int?)model.SystemType.Value : null);
        }

        public static ContactTypeModel GetViewModel(int? contactTypeID, Guid organizationID, EntityContext context)
        {
            ContactTypeModel model = new ContactTypeModel() { OrganizationID = organizationID };
            if (contactTypeID != null && contactTypeID != 0)
            {
                var contactType = context.ContactTypes.FirstOrDefault(c => c.ID == contactTypeID);
                if (contactType == null)
                    throw new InvalidOperationException("Contact Type with ID = {0} does not exist anymore".ToFormat(contactTypeID));

                model.ID = contactType.ID;
                model.OrganizationID = organizationID;
                model.Name = contactType.Name;
                model.DisplayPattern = contactType.DisplayPattern;
                model.SystemType = (SystemContactType)contactType.SystemType;
            }
            return model;
        }

        public static ContactType SetContactType(ContactTypeModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            ContactType contactType = null;

            if (model.ID != 0)
            {
                contactType = context.ContactTypes.FirstOrDefault(c => c.ID == model.ID);

                if (contactType == null)
                    throw new InvalidOperationException("Contact Type with ID = {0} does not exist anymore".ToFormat(model.ID));
            }
            else
            {
                contactType = new ContactType();
                context.ContactTypes.AddObject(contactType);
            }

            UpdateContactTypeFromModel(contactType, model, context);
            return contactType;
        }

        public static void DeleteContactType(int contactTypeID, EntityContext context)
        {
            ContactType contactType = context.ContactTypes.FirstOrDefault(c => c.ID == contactTypeID);
            if (contactType.Contacts.Any())
                throw new InvalidOperationException("There are contacts with this Contact Type. Deletion aborted");

            if (contactType != null)
            {
                context.ContactTypes.DeleteObject(contactType);
            }
        }

        public static ContactType AddNew(string name, Guid organizationID, EntityContext context)
        {
            ContactType contactType = new ContactType() { OrganizationID = organizationID, Name = name };
            context.ContactTypes.AddObject(contactType);
            return contactType;
        }
    }
}