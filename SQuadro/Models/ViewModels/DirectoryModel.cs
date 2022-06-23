using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SQuadro.Models
{
    public abstract class DirectoryModel
    {
        public Int64 ID { get; set; }
        public Guid OrganizationID { get; set; }
        public string Name { get; set; }
    }

    public class ContactTypeModel : DirectoryModel
    {
        [Display(Name="Display Pattern")]
        [AllowHtml]
        public string DisplayPattern { get; set; }

        public SystemContactType SystemType { get; set; }
    }

    public class DocumentTypeModel : DirectoryModel
    {
    }

    public class DocumentStatusModel : DirectoryModel
    {
    }

    public class TagModel : DirectoryModel
    {
        [Required]
        [StringLength(32)]
        public new string Name { get; set; }
    }

    public class VesselModel : DirectoryModel
    {
        public new Guid ID { get; set; }
    }
}