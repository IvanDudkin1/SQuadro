using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SQuadro.Models
{
    public class AreaModel
    {
        public Guid ID { get; set; }

        public Guid OrganizationID { get; set; }

        [Required]
        [Display(Name = "Area Name")]
        public string Name { get; set; }
    }
}