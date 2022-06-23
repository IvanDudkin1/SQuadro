using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace SQuadro.Models
{
    public class PartnerModel
    {
        public Guid ID { get; set; }

        public Guid OrganizationID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Categories")]
        public string Categories { get; set; }

        [Display(Name = "Areas")]
        public string Areas { get; set; }

        [Display(Name = "Contacts")]
        public IList<ContactModel> Contacts { get; set; }

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