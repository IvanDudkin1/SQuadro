using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class DocumentSetsList : BaseListTemplate
    {
        public DocumentSetsList(IUsersHelper IUsersHelper)
            : base("DocumentSets")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/documentSetsList.js";
            JavaScriptClassName = "documentSetsList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/DocumentSets/DataSourceCallback");

            this.Name = "Document Sets";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "documentSetsList.addNew()";
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
            var types = EntityContext.Current.DocumentSets.Where(ds => ds.OrganizationID == ParentID).Select(ds =>
                    new {
                        ID = ds.ID
                        , Name = ds.Name
                    });
            totalRecords = types.Count();

            return DataTableProcessor.ProcessTable(param, types, out filteredRecords, Columns);
        }
    }
}
