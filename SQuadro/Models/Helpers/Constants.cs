using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SQuadro.Models
{
    public static class Constants
    {
        public static string ProgramName = ConfigurationManager.AppSettings["ProgramName"];
        public static string TechSupportContactDetails = ConfigurationManager.AppSettings["TechSupportContactDetails"];

        public static string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];
    }
}