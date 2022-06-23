using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class ListTemplateFilterProperties
    {
        private List<ListTemplateFilter> filters = new List<ListTemplateFilter>();

        public ListTemplateFilterProperties(string postfix, params ListTemplateFilter[] filters)
        {
            foreach (ListTemplateFilter currentFilter in filters)
            {
                if (!this.filters.Contains(currentFilter))
                    this.filters.Add(currentFilter);
            }
            this.Postfix = postfix;
        }

        public IEnumerable<ListTemplateFilter> Filters { get { return filters; } }

        public string Postfix { get; private set; }              
    }
}