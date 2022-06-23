using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SQuadro.Models
{
    public class UserModel
    {
        public Guid ID { get; set; }

        public Guid OrganizationID { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required]
        [EmailValidation]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "System Role")]
        public int SystemRole { get; set; }

        [Display(Name = "User Role")]
        public Guid? UserRoleID { get; set; }
    }
}