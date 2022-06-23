using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class MainMenu
    {
        private MainMenu()
        { }

        private MainMenu(string value, string text, string resourceKey)
        {
            Value = value;
            Text = text;
            ResourceKey = resourceKey;
            instance[value] = this;
        }

        private static readonly Dictionary<string, MainMenu> instance = new Dictionary<string, MainMenu>();

        public string Value { get; private set; }
        public string Text { get; private set; }
        public string ResourceKey { get; private set; }

        public static explicit operator MainMenu(string value)
        {
            MainMenu result;
            if (instance.TryGetValue(value, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        public override String ToString()
        {
            return Value;
        }

        public static MainMenu Dashboard = new MainMenu("mainDashboard", "Dashboard", "mnu_Dashboard");
        public static MainMenu CompaniesGroup = new MainMenu("groupCompanies", "Companies", "mnu_Companies");
        public static MainMenu DocumentsGroup = new MainMenu("groupDocuments", "Documents", "mnu_Documents");
        public static MainMenu RelatedObjectsGroup = new MainMenu("groupRelatedObjects", "Relations", "mnu_RelatedObjects");
        public static MainMenu SettingsGroup = new MainMenu("groupSettings", "Settings", "mnu_Settings");
        public static MainMenu AdministrationGroup = new MainMenu("groupAdministration", "Administration", "mnu_Administration");
        public static MainMenu Companies = new MainMenu("itemCompanies", "Companies", "mnu_Companies");
        public static MainMenu Areas = new MainMenu("itemAreas", "Areas", "mnu_Areas");
        public static MainMenu Categories = new MainMenu("itemCategories", "Categories", "mnu_Categories");
        public static MainMenu ContactTypes = new MainMenu("itemContactTypes", "Contact Types", "mnu_ContactTypes");
        public static MainMenu Documents = new MainMenu("itemDocuments", "Documents", "mnu_Documents");
        public static MainMenu DocumentSets = new MainMenu("itemDocumentSets", "Document Sets", "mnu_DocumentSets");
        public static MainMenu DocumentTypes = new MainMenu("itemDocumentTypes", "Types", "mnu_DocumentTypes");
        public static MainMenu DocumentStatuses = new MainMenu("itemDocumentStatuses", "Statuses", "mnu_DocumentStatuses");
        public static MainMenu DocumentTags = new MainMenu("itemDocumentTags", "Tags", "mnu_DocumentTags");
        public static MainMenu Vessels = new MainMenu("itemVessels", "Vessels", "mnu_Vessels");
        public static MainMenu EmailSettings = new MainMenu("itemEmailSettings", "Email Settings", "mnu_EmailSettings");
        public static MainMenu Subjects = new MainMenu("itemSubjects", "Subjects", "mnu_Subjects");
        public static MainMenu EmailTemplates = new MainMenu("itemEmailTemplates", "Email Templates", "mnu_EmailTemplates");
        public static MainMenu Users = new MainMenu("itemUsers", "Users", "mnu_Users");
        public static MainMenu UserRoles = new MainMenu("itemUserRoles", "User Roles", "mnu_UserRoles");
        public static MainMenu MyAccount = new MainMenu("groupMyAccount", "My Account", "mnu_MyProfile");
    }
}