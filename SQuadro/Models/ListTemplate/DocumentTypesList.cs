using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class DocumentTypesList : BaseListTemplate
    {
        public DocumentTypesList(IUsersHelper IUsersHelper)
            : base("DocumentTypes")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/documentTypesList.js";
            JavaScriptClassName = "documentTypesList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/DocumentTypes/DataSourceCallback");

            this.Name = "Document Types";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "documentTypesList.addNew()";
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
            var types = EntityContext.Current.DocumentTypes.Where(dt => dt.OrganizationID == ParentID).Select(dt =>
                    new {
                        ID = dt.ID
                        , Name = dt.Name
                    });
            totalRecords = types.Count();

            return DataTableProcessor.ProcessTable(param, types, out filteredRecords, Columns);
        }
    }
}
