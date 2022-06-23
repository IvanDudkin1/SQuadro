using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SQuadro.Filters;

namespace SQuadro.Models
{
    public class DocumentModel
    {
        public Guid ID { get; set; }

        public Guid OrganizationID { get; set; }

        [RequiredIf("ID == Guid.Empty", ErrorMessage="The File field is required")]
        [DisplayName("File")]
        public HttpPostedFileBase DocumentFile { get; set; }

        public string FileName { get; set; }

        [Required]
        public string Name { get; set; }

        public string Number { get; set; }

        [Required]
        [DisplayName("Document Date")]
        public DateTime Date { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Document Type")]
        public Int32? DocumentTypeID { get; set; }

        [DisplayName("Document Type")]
        public string DocumentType { get; set; }

        [DisplayName("Document Status")]
        public Int32? DocumentStatusID { get; set; }

        [DisplayName("Tags")]
        public string Tags { get; set; }

        [DisplayName("Related Object")]
        public Guid? RelatedObjectID { get; set; }

        [DisplayName("Document Sets")]
        public string DocumentSets { get; set; }

        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        public DateTimeOffset CreatedOn { get; set; }

        [DisplayName("Updated By")]
        public string UpdatedBy { get; set; }

        [DisplayName("Updated On")]
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}