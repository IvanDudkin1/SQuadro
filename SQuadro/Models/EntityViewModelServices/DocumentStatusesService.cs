using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class DocumentStatusesService
    {
        private static void UpdateDocumentStatusFromModel(DocumentStatus documentStatus, DocumentStatusModel model, EntityContext context)
        {
            documentStatus.OrganizationID = model.OrganizationID;
            documentStatus.Name = model.Name;
        }

        public static DocumentStatusModel GetViewModel(int? documentStatusID, Guid organizationID, EntityContext context)
        {
            DocumentStatusModel model = new DocumentStatusModel() { OrganizationID = organizationID };
            if (documentStatusID != null && documentStatusID != 0)
            {
                var documentStatus = context.DocumentStatuses.FirstOrDefault(c => c.ID == documentStatusID);
                if (documentStatus == null)
                    throw new InvalidOperationException("Document Status with ID = {0} does not exist anymore".ToFormat(documentStatusID));

                model.ID = documentStatus.ID;
                model.OrganizationID = documentStatus.OrganizationID;
                model.Name = documentStatus.Name;
            }
            return model;
        }

        public static DocumentStatus SetDocumentStatus(DocumentStatusModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            DocumentStatus documentStatus = null;

            if (model.ID != 0)
            {
                documentStatus = context.DocumentStatuses.FirstOrDefault(c => c.ID == model.ID);

                if (documentStatus == null)
                    throw new InvalidOperationException("Document Status with ID = {0} does not exist anymore".ToFormat(model.ID));
            }
            else
            {
                documentStatus = new DocumentStatus();
                context.DocumentStatuses.AddObject(documentStatus);
            }

            UpdateDocumentStatusFromModel(documentStatus, model, context);
            return documentStatus;
        }

        public static void DeleteDocumentStatus(int documentStatusID, EntityContext context)
        {
            DocumentStatus documentStatus = context.DocumentStatuses.FirstOrDefault(c => c.ID == documentStatusID);
            if (documentStatus.Documents.Any())
                throw new InvalidOperationException("There are Documents with this Document Status. Deletion aborted");

            if (documentStatus != null)
            {
                context.DocumentStatuses.DeleteObject(documentStatus);
            }
        }

        public static DocumentStatus AddNew(string name, Guid organizationID, EntityContext context)
        {
            DocumentStatus documentStatus = new DocumentStatus() { OrganizationID = organizationID, Name = name };
            context.DocumentStatuses.AddObject(documentStatus);
            return documentStatus;
        }
    }
}