using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQuadro.Models
{
    public class DateFilterSettings
    {
        public bool Visible { get; set; }
        public string NameFrom { get; set; }
        public string NameTo { get; set; } 
        public string CaptionFrom { get; set; }
        public string CaptionTo { get; set; }
        public DateTime ValueFrom { get; set; }
        public DateTime ValueTo { get; set; }
    }
}
