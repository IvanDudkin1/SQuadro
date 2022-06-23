using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class Column
    {
        public string Name { get; set; }
        public FilterType FilterType { get; set; }
        public bool IsSelector { get; set; }
    }

    public enum FilterType
    {
        General,
        Date,
        Numeric,
        None
    }
}