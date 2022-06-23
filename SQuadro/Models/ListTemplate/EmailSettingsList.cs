using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class EmailSettingsList : BaseListTemplate
    {
        public EmailSettingsList(IUsersHelper IUsersHelper)
            : base("EmailSettings")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/emailSettingsList.js";
            JavaScriptClassName = "emailSettingsList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/EmailSettings/DataSourceCallback");

            this.Name = "Email Settings";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "emailSettingsList.addNew()";
            }

            Columns = new List<Column>() {
                new Column() { Name = "ID", FilterType = FilterType.None }, 
                new Column() { Name = "Name", FilterType = FilterType.General },
                new Column() { Name = "Email", FilterType = FilterType.General },
                new Column() { Name = "IsDefault", FilterType = FilterType.None },
                new Column() { Name = "Actions", FilterType = FilterType.None },
            };

            DefaultSorting = new object[] { new object[] { 3, "desc" }, new object[] { 1, "asc" } };
        }

        public override object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords)
        {
            var emailSettings = EntityContext.Current.EmailSettings.Where(e => e.OrganizationID == ParentID).Select(e =>
                    new
                    {
                        ID = e.ID,
                        Name = e.Name,
                        Email = e.Email,
                        IsDefault = e.IsDefault
                    });
            totalRecords = emailSettings.Count();

            return DataTableProcessor.ProcessTable(param, emailSettings, out filteredRecords, Columns);
        }
    }
}
