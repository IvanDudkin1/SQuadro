using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SQuadro.Models
{
    public class DocumentSetModel
    {
        public Guid ID { get; set; }

        public Guid OrganizationID { get; set; }

        [Required]
        [StringLength(256)]
        [DisplayAttribute(Name="Name")]
        public string Name { get; set; }

        [DisplayAttribute(Name = "Documents")]
        public string Documents { get; set; }

        public IEnumerable<Guid> DocumentIDs
        {
            get 
            {
                Guid tmpID = Guid.Empty;
                return this.Documents.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID);
            }
        }
    }
}