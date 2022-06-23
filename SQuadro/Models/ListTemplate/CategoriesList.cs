using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class CategoriesList : BaseListTemplate
    {
        public CategoriesList(IUsersHelper IUsersHelper)
            : base("Categories")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/categoriesList.js";
            JavaScriptClassName = "categoriesList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/Categories/DataSourceCallback");

            this.Name = "Categories";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "categoriesList.addNew()";
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
            var categories = EntityContext.Current.Categories
                .Where(c => c.OrganizationID == ParentID && currentUser.AvailableCategories.Contains(c.ID))
                .Select(c =>
                    new {
                        ID = c.ID
                        , Name = c.Name
                    });
            totalRecords = categories.Count();

            return DataTableProcessor.ProcessTable(param, categories, out filteredRecords, Columns);
        }
    }
}
