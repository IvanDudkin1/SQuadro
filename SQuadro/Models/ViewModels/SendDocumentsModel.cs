using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SQuadro.Models
{
    public class SendDocumentsModel
    {
        [Display(Name = "Sender")]
        public Guid? Sender { get; set; }

        [Required]
        [Display(Name = "Documents")]
        public string DocumentsSelection { get; set; }

        [Required]
        [Display(Name = "Recipients")]
        public string RecipientsSelection { get; set; }
        
        public bool SendAsanAttachment { get; set; }

        [Display(Name = "Subject")]
        [StringLength(256)]
        public string Subject { get; set; }
    }
}