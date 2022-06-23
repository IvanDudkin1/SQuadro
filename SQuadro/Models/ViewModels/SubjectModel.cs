using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class SubjectModel
    {
        public Int64 ID { get; set; }
        public Guid OrganizationID { get; set; }

        [Required]
        [StringLength(256)]
        public string Text { get; set; }
    }
}