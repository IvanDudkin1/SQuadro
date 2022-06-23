using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class DocumentStatusesList : BaseListTemplate
    {
        public DocumentStatusesList(IUsersHelper IUsersHelper)
            : base("DocumentStatuses")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/documentStatusesList.js";
            JavaScriptClassName = "documentStatusesList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/DocumentStatuses/DataSourceCallback");

            this.Name = "Document Statuses";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "documentStatusesList.addNew()";
            }

            Columns = new List<Column>() {
                new Column() { Name = "ID", FilterType = FilterType.None }, 
                new Column() { Name = "Name", FilterType = FilterType.General }, 
                new Column() { Name = "Actions", FilterType = FilterType.None },
            };

            DefaultSorting = new object[] { new object[] { 1, "asc" } };
        }

        public override object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords)
        {
            var statuses = EntityContext.Current.DocumentStatuses.Where(ds => ds.OrganizationID == ParentID).Select(ds =>
                    new {
                        ID = ds.ID
                        , Name = ds.Name
                    });
            totalRecords = statuses.Count();

            return DataTableProcessor.ProcessTable(param, statuses, out filteredRecords, Columns);
        }
    }
}
