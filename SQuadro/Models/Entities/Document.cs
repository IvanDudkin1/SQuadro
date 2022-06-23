using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro
{
    public partial class Document
    {
        public void Delete(EntityContext context)
        {
            if (this.DocumentSets.Any())
                foreach (var documentSet in this.DocumentSets.ToList())
                    documentSet.Documents.Remove(this);
            context.Documents.DeleteObject(this);
        }
    }
}