using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQuadro.Models
{
    public class UploadFileSettings
    {
        public bool Visible { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UploadScript { get; set; }
    }
}
