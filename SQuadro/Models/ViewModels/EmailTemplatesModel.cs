using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SQuadro.Models
{
    public class EmailTemplatesModel
    {
        public Guid ID { get; set; }

        public Guid OrganizationID { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int Type { get; set; }

        [Required]
        [Display(Name = "Subject")]
        [StringLength(256)]
        public string Subject { get; set; }

        [Display(Name = "Salutation")]
        [StringLength(128)]
        public string Salutation { get; set; }

        [Display(Name = "Body")]
        public string Body { get; set; }

        [Display(Name = "Signature")]
        [StringLength(128)]
        public string Signature { get; set; }
    }
}