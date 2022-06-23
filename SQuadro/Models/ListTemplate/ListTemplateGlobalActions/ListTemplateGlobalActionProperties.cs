using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class ListTemplateGlobalActionProperties
    {
        private List<ListTemplateGlobalAction> _Actions = new List<ListTemplateGlobalAction>();

        public ListTemplateGlobalActionProperties(string postfix, params ListTemplateGlobalAction[] actions)
        {
            foreach (ListTemplateGlobalAction action in actions)
            {
                if (!_Actions.Contains(action))
                    _Actions.Add(action);
            }
            this.Postfix = postfix;
        }

        public IEnumerable<ListTemplateGlobalAction> Actions { get { return _Actions; } }

        public string Postfix { get; private set; }
    }
}