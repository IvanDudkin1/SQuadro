using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;

namespace SQuadro.Models
{
    public class ListTemplateGlobalActionsSettings
    {
        private OrderedDictionary _Settigns = new OrderedDictionary();

        public ListTemplateGlobalActionsSettings(ListTemplateGlobalActionProperties properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            this.Properties = properties;

            foreach (ListTemplateGlobalAction action in properties.Actions)
                _Settigns.Add(action, GenerateGlobalActionSettings(action, properties.Postfix));
        }

        public ListTemplateGlobalActionProperties Properties { get; private set; }

        public GlobalActionsSetting this[ListTemplateGlobalAction action] { get { return (GlobalActionsSetting)_Settigns[action]; } }

        public IList<GlobalActionsSetting> GlobalActionSettingsList
        {
            get { return _Settigns.Cast<DictionaryEntry>().Select(s => (GlobalActionsSetting)s.Value).ToList(); }
        }

        public GlobalActionsSetting GlobalAction1Settings
        {
            get { return getSettingByIndex(0); }
        }

        public GlobalActionsSetting GlobalAction2Settings
        {
            get { return getSettingByIndex(1); }
        }

        public GlobalActionsSetting GlobalAction3Settings
        {
            get { return getSettingByIndex(2); }
        }

        public GlobalActionsSetting GlobalAction4Settings
        {
            get { return getSettingByIndex(3); }
        }

        private GlobalActionsSetting GenerateGlobalActionSettings(ListTemplateGlobalAction action, string postfix)
        {
            ActionButtonSettings buttonSettings = new ActionButtonSettings()
            {
                Name = "ltGlobalAction{0}_{1}".ToFormat(action.ToString(), postfix),
            };
            return new GlobalActionsSetting(buttonSettings);
        }

        private GlobalActionsSetting getSettingByIndex(int index)
        {
            if (_Settigns.Count - 1 < index)
                return null;
            else
                return (GlobalActionsSetting)_Settigns[index];
        }

        public class GlobalActionsSetting
        {
            public GlobalActionsSetting(ActionButtonSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings", "you must supply the settings!");
                }

                this.ButtonSettings = settings;
            }

            public ActionButtonSettings ButtonSettings { get; private set; }
        }
    }
}