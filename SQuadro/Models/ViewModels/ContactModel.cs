using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SQuadro.Models
{
    public class ContactModel
    {
        public Guid ID { get; set; }

        public Guid CompanyID { get; set; }

        [Required]
        [Display(Name = "Contact Type")]
        public int? Type { get; set; }

        public string TypeName { get; set; }

        [Required]
        [Display(Name = "Data")]
        public string Data { get; set; }

        public string ContactDisplay { get; set; }

        [Required]
        [Display(Name = "Primary Contact")]
        public bool IsPrimary { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}