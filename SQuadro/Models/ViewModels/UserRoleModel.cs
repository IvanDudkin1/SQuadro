using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class UserRoleModel
    {
        public Guid ID { get; set; }
        public Guid OrganizationID { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Readonly Access")]
        public bool IsReadOnly { get; set; }

        [Display(Name = "Access to Companies")]
        public string Categories { get; set; }

        [Display(Name = "Access to Documents")]
        public string RelatedObjects { get; set; }
    }
}