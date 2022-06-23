using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class DocumentTypesService
    {
        private static void UpdateDocumentTypeFromModel(DocumentType documentType, DocumentTypeModel model, EntityContext context)
        {
            documentType.OrganizationID = model.OrganizationID;
            documentType.Name = model.Name;
        }

        public static DocumentTypeModel GetViewModel(int? documentTypeID, Guid organizationID, EntityContext context)
        {
            DocumentTypeModel model = new DocumentTypeModel() { OrganizationID = organizationID };
            if (documentTypeID != null && documentTypeID != 0)
            {
                var documentType = context.DocumentTypes.FirstOrDefault(c => c.ID == documentTypeID);
                if (documentType == null)
                    throw new InvalidOperationException("Document Type with ID = {0} does not exist anymore".ToFormat(documentTypeID));

                model.ID = documentType.ID;
                model.OrganizationID = documentType.OrganizationID;
                model.Name = documentType.Name;
            }
            return model;
        }

        public static DocumentType SetDocumentType(DocumentTypeModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            DocumentType documentType = null;

            if (model.ID != 0)
            {
                documentType = context.DocumentTypes.FirstOrDefault(c => c.ID == model.ID);

                if (documentType == null)
                    throw new InvalidOperationException("Document Type with ID = {0} does not exist anymore".ToFormat(model.ID));
            }
            else
            {
                documentType = new DocumentType();
                context.DocumentTypes.AddObject(documentType);
            }

            UpdateDocumentTypeFromModel(documentType, model, context);
            return documentType;
        }

        public static void DeleteDocumentType(int documentTypeID, EntityContext context)
        {
            DocumentType documentType = context.DocumentTypes.FirstOrDefault(c => c.ID == documentTypeID);
            if (documentType.Documents.Any())
                throw new InvalidOperationException("There are Documents with this Document Type. Deletion aborted");

            if (documentType != null)
            {
                context.DocumentTypes.DeleteObject(documentType);
            }
        }

        public static DocumentType AddNew(string name, Guid organizationID, EntityContext context)
        {
            DocumentType documentType = new DocumentType() { OrganizationID = organizationID, Name = name };
            context.DocumentTypes.AddObject(documentType);
            return documentType;
        }
    }
}