using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class DocumentSetsService
    {
        private static void UpdateDocumentSetFromModel(DocumentSet documentSet, DocumentSetModel model, EntityContext context)
        {
            documentSet.OrganizationID = model.OrganizationID;
            documentSet.Name = model.Name;

            foreach (Guid id in model.DocumentIDs)
            {
                if (!documentSet.Documents.Any(d => d.ID == id))
                {
                    var document = context.Documents.FirstOrDefault(d => d.ID == id);
                    if (document != null)
                        documentSet.Documents.Add(document);
                }
            }

            foreach (var document in documentSet.Documents.ToList())
            {
                if (!model.DocumentIDs.Contains(document.ID))
                    documentSet.Documents.Remove(document);
            }
        }

        public static DocumentSetModel GetViewModel(Guid? documentSetID, Guid[] documents, Guid organizationID, EntityContext context)
        {
            DocumentSetModel model = new DocumentSetModel() { OrganizationID = organizationID };
            if (documents != null && documents.Length > 0)
                model.Documents = String.Join(",", documents);
            if (documentSetID != null && documentSetID != Guid.Empty)
            {
                var documentSet = context.DocumentSets.FirstOrDefault(c => c.ID == documentSetID);
                if (documentSet == null)
                    throw new InvalidOperationException("Document Set with ID = {0} does not exist anymore".ToFormat(documentSetID));

                model.ID = documentSet.ID;
                model.OrganizationID = documentSet.OrganizationID;
                model.Name = documentSet.Name;
                if (documentSet.Documents.Any())
                    model.Documents = documentSet.Documents.Select(d => d.ID.ToString()).Aggregate((id1, id2) => id1 + ", " + id2);
            }
            return model;
        }

        public static DocumentSet SetDocumentSet(DocumentSetModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (context.DocumentSets.Any(ds => ds.OrganizationID == model.OrganizationID && ds.Name == model.Name && ds.ID != model.ID))
                throw new InvalidOperationException("Document Set with name {0} already exists in the datatabase".ToFormat(model.Name));

            DocumentSet documentSet = null;

            if (model.ID != Guid.Empty)
            {
                documentSet = context.DocumentSets.FirstOrDefault(c => c.ID == model.ID);

                if (documentSet == null)
                    throw new InvalidOperationException("Document Set with ID = {0} does not exist anymore".ToFormat(model.ID));
            }
            else
            {
                documentSet = new DocumentSet();
                context.DocumentSets.AddObject(documentSet);
            }

            UpdateDocumentSetFromModel(documentSet, model, context);
            return documentSet;
        }

        public static void DeleteDocumentSet(Guid documentSetID, EntityContext context)
        {
            DocumentSet documentSet = context.DocumentSets.FirstOrDefault(c => c.ID == documentSetID);
            if (documentSet != null)
            {
                foreach (var document in documentSet.Documents.ToList())
                    documentSet.Documents.Remove(document);

                context.DocumentSets.DeleteObject(documentSet);
            }
        }

        public static DocumentSet AddNew(string name, Guid organizationID, EntityContext context)
        {
            var documentSet = context.DocumentSets.FirstOrDefault(
                ds => ds.OrganizationID == organizationID && ds.Name.ToUpper().Trim() == name.ToUpper().Trim());
            if (documentSet == null)
                documentSet = new DocumentSet() { OrganizationID = organizationID, Name = name };
            context.DocumentSets.AddObject(documentSet);
            return documentSet;
        }
    }
}