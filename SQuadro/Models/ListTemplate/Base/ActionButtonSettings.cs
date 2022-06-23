using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class ActionButtonSettings
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public string Click { get; set; }
        public bool IsDropdownButton { get; set; }

        private List<ActionButtonSettings> dropdownButtonSettings = new List<ActionButtonSettings>();
        public IList<ActionButtonSettings> DropdownButtonSettings { get { return dropdownButtonSettings; } }

        private bool visible = true;
        public bool Visible { get { return visible; } set { visible = value; } }
    }
}