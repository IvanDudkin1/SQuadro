using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace SQuadro.Models
{
    public class ListTemplateFiltersSettings
    {
        public ListTemplateFiltersSettings(ListTemplateFilterProperties properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            this.Properties = properties;

            foreach (ListTemplateFilter currentFilter in properties.Filters)
                settings.Add(currentFilter, generateFilterSettings(currentFilter, properties.Postfix));
            
        }

        private OrderedDictionary settings = new OrderedDictionary();

        public string JsonFilterSettings { 
            get 
            { 
                object[] arr = {};
                Array.Resize(ref arr, settings.Count);
                this.settings.Values.CopyTo(arr, 0);
                return Newtonsoft.Json.JsonConvert.SerializeObject(arr.Select(i => new { Name = ((FilterSetting)i).Name }));
            } 
        }

        public ListTemplateFilterProperties Properties { get; private set; }

        public FilterSetting this[ListTemplateFilter filter] { get { return (FilterSetting)settings[filter]; } }

        public FilterSetting Filter1Settings
        {
            get { return GetSettingByIndex(0); }
        }

        public FilterSetting Filter2Settings
        {
            get { return GetSettingByIndex(1); }
        }

        public FilterSetting Filter3Settings
        {
            get { return GetSettingByIndex(2); }
        }

        public FilterSetting Filter4Settings
        {
            get { return GetSettingByIndex(3); }
        }
        
        private FilterSetting generateFilterSettings(ListTemplateFilter filter, string postfix)
        {
            return new FilterSetting(filter.ToString(), postfix);
        }

        private FilterSetting GetSettingByIndex(int index)
        {
            if (settings.Count - 1 < index)
                return null;
            else
                return (FilterSetting)settings[index];
        }

        public class FilterSetting
        {
            internal FilterSetting(string name, string postfix)
            {
                Name = "ltFilter{0}_{1}".ToFormat(name, postfix);
            }
            
            public string Name { get; set; }
            public IEnumerable<SelectListItem> DataSource { get; set; }
            public object SelectedValue { get; set; }
        }
    }
}