using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SQuadro.Models
{
    public class EmailSettingsModel
    {
        public Guid ID { get; set; }

        public Guid OrganizationID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(256)]
        [AllowHtml]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        [StringLength(256)]
        [EmailValidation]
        public string Email { get; set; }

        [Display(Name = "Default")]
        public bool IsDefault { get; set; }

        [Required]
        [Display(Name = "Smtp Server")]
        [StringLength(256)]
        public string SmtpServer { get; set; }

        [Required]
        [Display(Name = "Smtp Port")]
        public int SmtpPort { get; set; }

        [Required]
        [Display(Name = "Smtp User")]
        [StringLength(256)]
        public string SmtpUser { get; set; }

        [Required]
        [Display(Name = "Smtp Password")]
        [StringLength(256)]
        public string SmtpPassword { get; set; }

        [Required]
        [Display(Name = "Enable Ssl")]
        public bool EnableSsl { get; set; }
    }
}