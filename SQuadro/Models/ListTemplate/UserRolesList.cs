using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class UserRolesList : BaseListTemplate
    {
        public UserRolesList(IUsersHelper IUsersHelper)
            : base("UserRoles")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/userRolesList.js";
            JavaScriptClassName = "userRolesList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/UserRoles/DataSourceCallback");

            this.Name = "User Roles";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "userRolesList.addNew()";
            }

            Columns = new List<Column>() {
                new Column() { Name = "ID", FilterType = FilterType.None },
                new Column() { Name = "Name", FilterType = FilterType.General },
                new Column() { Name = "IsReadonly", FilterType = FilterType.None }, 
                new Column() { Name = "CompaniesAccess", FilterType = FilterType.General },
                new Column() { Name = "DocumentsAccess", FilterType = FilterType.General },
                new Column() { Name = "Actions", FilterType = FilterType.None },
            };

            DefaultSorting = new object[] { new object[] { 1, "asc" } };
        }

        public override object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords)
        {
            var useRoles = EntityContext.Current.UserRoles
                .AsEnumerable()
                .Where(ur => ur.OrganizationID == ParentID)
                .Select(ur =>
                    new {
                        ID = ur.ID
                        , Name = ur.Name
                        , IsReadonly = ur.IsReadonly
                        , CompaniesAccess = ur.Categories.Any() ? ur.Categories.Select(c => c.Name).Aggregate((cn1, cn2) => cn1 + ", " + cn2) : "All Companies"
                        , DocumentsAccess = ur.RelatedObjects.Any() ? ur.RelatedObjects.Select(ro => ro.Name).Aggregate((ro1, ro2) => ro1 + ", " + ro2) : "All Documents"
                    });
            totalRecords = useRoles.Count();

            return DataTableProcessor.ProcessTable(param, useRoles.AsQueryable(), out filteredRecords, Columns);
        }
    }
}
