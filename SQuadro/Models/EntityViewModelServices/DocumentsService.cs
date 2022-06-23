using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace SQuadro.Models
{
    public static class DocumentsService
    {
        private static void UpdateDocumentFromModel(Document document, User currentUser, DocumentModel model, EntityContext context)
        {
            document.OrganizationID = model.OrganizationID;
            document.Tags = model.Tags;
            document.Comment = model.Comment;
            document.Date = model.Date;
            document.ExpirationDate = model.ExpirationDate;
            document.DocumentStatusID = model.DocumentStatusID;
            document.DocumentTypeID = model.DocumentTypeID;
            document.Name = model.Name;
            document.Number = model.Number;
            document.RelatedObjectID = model.RelatedObjectID;
            if (model.DocumentFile != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    model.DocumentFile.InputStream.CopyTo(stream);
                    document.File = stream.ToArray();
                    document.FileName = model.DocumentFile.FileName;
                }
            }

            IEnumerable<Guid> documentSetIDs = new List<Guid>();
            if (model.DocumentSets != null) documentSetIDs = model.DocumentSets.ToGuidArray();
            foreach (Guid id in documentSetIDs)
            {
                if (!document.DocumentSets.Any(ds => ds.ID == id))
                {
                    var documentSet = context.DocumentSets.FirstOrDefault(ds => ds.ID == id);
                    if (documentSet != null)
                        document.DocumentSets.Add(documentSet);
                }
            }
            foreach (var documentSet in document.DocumentSets.ToList())
            {
                if (!documentSetIDs.Contains(documentSet.ID))
                    document.DocumentSets.Remove(documentSet);
            }

            if (model.ID == Guid.Empty)
            {
                document.CreatedByUserID = currentUser.ID;
                document.CreatedOn = DateTimeOffset.Now;
            }
            else
            {
                document.UpdatedByUserID = currentUser.ID;
                document.UpdatedOn = DateTimeOffset.Now;
            }
        }

        public static string DocumentsRootPath = ConfigurationManager.AppSettings["DocumentsRootPath"];

        public static Document GetDocument(Guid id, EntityContext context)
        {
            var document = context.Documents.SingleOrDefault(d => d.ID == id);
            if (document == null)
                throw new InvalidOperationException("Document with ID = {0} does not exist anymore".ToFormat(id));
            return document;
        }

        public static string GetFullPath(Guid organizationID, string fileName)
        {
            return Path.Combine(DocumentsRootPath, organizationID.ToString(), fileName);
        }

        public static DocumentModel GetViewModel(Guid? documentID, Guid organizationID, EntityContext context)
        {
            DocumentModel model = new DocumentModel() { OrganizationID = organizationID, Date = DateTime.Today };
            if (documentID != null && documentID != Guid.Empty)
            {
                var document = GetDocument(documentID.Value, context);

                model.ID = document.ID;
                model.OrganizationID = organizationID;
                model.FileName = document.FileName;
                model.Name = document.Name;
                model.Comment = document.Name;
                model.Comment = document.Comment;
                model.Date = document.Date;
                model.DocumentStatusID = document.DocumentStatusID;
                model.DocumentTypeID = document.DocumentTypeID;
                model.DocumentType = document.DocumentType != null ? document.DocumentType.Name : null;
                model.ExpirationDate = document.ExpirationDate;
                model.Number = document.Number;
                model.Tags = document.Tags;
                model.RelatedObjectID = document.RelatedObjectID;
                model.DocumentSets = document.DocumentSets.Select(ds => ds.ID).JoinToString();
                model.CreatedBy = document.UserCreatedBy.Name;
                model.CreatedOn = document.CreatedOn.ToTimeZone(document.Organization.Timezone);
                model.UpdatedBy = document.UpdatedByUserID.HasValue ? document.UserUpdatedBy.Name : String.Empty;
                model.UpdatedOn = (document.UpdatedOn.HasValue ? document.UpdatedOn.Value.ToTimeZone(document.Organization.Timezone) : (DateTimeOffset?)null);
            }
            return model;
        }

        public static Document SetDocument(DocumentModel model, User currentUser, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Document document = null;

            if (model.ID != Guid.Empty)
            {
                document = GetDocument(model.ID, context);
            }
            else
            {
                document = new Document();
                context.Documents.AddObject(document);
            }

            UpdateDocumentFromModel(document, currentUser, model, context);
            return document;
        }

        public static void DeleteDocument(Guid id, User currentUser, EntityContext context)
        {
            var document = context.Documents.SingleOrDefault(d => d.ID == id);

            if (document == null) return;
            
            if (document.OrganizationID != currentUser.OrganizationID)
                throw new HttpException(403, "Access denied!");
            
            document.Delete(context);
        }

        public static void DeleteDocuments(Guid[] documents, User currentUser, EntityContext context)
        {
            if (documents != null)
                foreach (Guid id in documents)
                    DeleteDocument(id, currentUser, context);
        }
    }
}