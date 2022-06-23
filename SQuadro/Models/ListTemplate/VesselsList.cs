using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class VesselsList : BaseListTemplate
    {
        public VesselsList(IUsersHelper IUsersHelper)
            : base("Vessels")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/vesselsList.js";
            JavaScriptClassName = "vesselsList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/Vessels/DataSourceCallback");

            this.Name = "Vessels";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "vesselsList.addNew()";
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
            var types = EntityContext.Current.RelatedObjects
                .OfType<Vessel>()
                .Where(v => v.OrganizationID == ParentID && currentUser.AvailableRelatedObjects.Contains(v.ID))
                .Select(v =>
                    new {
                        ID = v.ID
                        , Name = v.Name
                    });
            totalRecords = types.Count();

            return DataTableProcessor.ProcessTable(param, types, out filteredRecords, Columns);
        }
    }
}
