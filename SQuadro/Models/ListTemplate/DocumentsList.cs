using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace SQuadro.Models
{
    public class DocumentsList : BaseListTemplate
    {
        public DocumentsList(IUsersHelper IUsersHelper)
            : base("Documents")
        {
            this.IUsersHelper = IUsersHelper;
        }

        IUsersHelper IUsersHelper;

        protected override void InitializeInternal()
        {
            User currentUser = IUsersHelper.CurrentUser;

            bool isSelectionList = false;
            if (this.InitParams != null)
                isSelectionList = (bool)this.InitParams.Item1;
             
            if (!isSelectionList)
                Name = "Documents";

            this.Readonly = currentUser.IsReadonly;

            JavaScriptFilePath = "~/Scripts/ListTemplate/{0}".ToFormat(isSelectionList ? "documentsSelectionList.js" : "documentsList.js");
            JavaScriptClassName = "documentsList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/Documents/DataSourceCallback");

            this.FiltersSettings = new ListTemplateFiltersSettings(
                new ListTemplateFilterProperties(this.Postfix,
                    ListTemplateFilter.DocumentTypes,
                    ListTemplateFilter.DocumentStatuses,
                    ListTemplateFilter.RelatedObjects,
                    ListTemplateFilter.DocumentSets));

            this.FiltersSettings[ListTemplateFilter.DocumentTypes].DataSource = ListsHelper.DocumentTypesList(currentUser.OrganizationID);
            this.FiltersSettings[ListTemplateFilter.DocumentTypes].SelectedValue = String.Empty;

            this.FiltersSettings[ListTemplateFilter.DocumentStatuses].DataSource = ListsHelper.DocumentStatusesList(currentUser.OrganizationID);
            this.FiltersSettings[ListTemplateFilter.DocumentStatuses].SelectedValue = String.Empty;

            this.FiltersSettings[ListTemplateFilter.RelatedObjects].DataSource = ListsHelper.RelatedObjectsList(currentUser);
            this.FiltersSettings[ListTemplateFilter.RelatedObjects].SelectedValue = Guid.Empty.ToString();

            this.FiltersSettings[ListTemplateFilter.DocumentSets].DataSource = ListsHelper.DocumentSetsList(currentUser.OrganizationID);
            this.FiltersSettings[ListTemplateFilter.DocumentSets].SelectedValue = String.Empty;

            if (!isSelectionList)
            {
                List<ListTemplateGlobalAction> actions = new List<ListTemplateGlobalAction>() { ListTemplateGlobalAction.Email, ListTemplateGlobalAction.DownloadAsZip };

                if (!this.Readonly)
                {
                    this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                        new ListTemplateGlobalActionProperties(this.Postfix,
                            ListTemplateGlobalAction.AddNew));

                    this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                    this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                    this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "documentsList.addNew()";

                    actions.InsertRange(1, new ListTemplateGlobalAction[] { ListTemplateGlobalAction.SaveAsSet, ListTemplateGlobalAction.Delete });
                }

                this.SelectionActionsSettings = new ListTemplateGlobalActionsSettings(new ListTemplateGlobalActionProperties(this.Postfix, actions.ToArray()));

                var action = this.SelectionActionsSettings[ListTemplateGlobalAction.Email];
                if (action != null)
                {
                    action.ButtonSettings.Text = "Send";
                    action.ButtonSettings.Html = "<i class=\"glyphicon glyphicon-envelope\"></i>";
                    action.ButtonSettings.Click = "documentsList.email()";
                }

                action = this.SelectionActionsSettings[ListTemplateGlobalAction.SaveAsSet];
                if (action != null)
                {
                    action.ButtonSettings.Text = "Save as Set";
                    action.ButtonSettings.Html = "<i class=\"glyphicon glyphicon-list-alt\"></i>";
                    action.ButtonSettings.Click = "documentsList.saveDocumentsSet()";
                }

                action = this.SelectionActionsSettings[ListTemplateGlobalAction.Delete];
                if (action != null)
                {
                    action.ButtonSettings.Text = "Delete";
                    action.ButtonSettings.Html = "<i class=\"glyphicon glyphicon-remove\"></i>";
                    action.ButtonSettings.Click = "documentsList.deleteDocuments()";
                }

                action = this.SelectionActionsSettings[ListTemplateGlobalAction.DownloadAsZip];
                if (action != null)
                {
                    action.ButtonSettings.Text = "Download as Zip";
                    action.ButtonSettings.Html = "<i class=\"glyphicon glyphicon-download-alt\"></i>";
                    action.ButtonSettings.Click = "documentsList.downloadDocuments()";
                }
            }

            this.Settings.AllowSelect = true;
            this.Settings.SuppressSelectionEvents = isSelectionList;

            Columns = new List<Column>() {
                new Column() { Name = "Selector", FilterType = FilterType.None, IsSelector = true },
                new Column() { Name = "ID", FilterType = FilterType.None }, 
                new Column() { Name = "Name", FilterType = FilterType.None }, 
                new Column() { Name = "Date", FilterType = FilterType.None },
                new Column() { Name = "Expiration", FilterType = FilterType.None },
                new Column() { Name = "Status", FilterType = FilterType.None },
                new Column() { Name = "Object", FilterType = FilterType.None },
                new Column() { Name = "Actions", FilterType = FilterType.None }
            };

            DefaultSorting = new object[] { new object[] { 2, "asc" } };
        }

        public override object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords)
        {
            User currentUser = IUsersHelper.CurrentUser;

            bool canViewAllRelatedObjects = currentUser.CanAddRelatedObject;

            string typeFilter = request[this.FiltersSettings[ListTemplateFilter.DocumentTypes].Name];
            int typeFilterID;
            bool typesFiltered = Int32.TryParse(typeFilter, out typeFilterID) && typeFilterID != 0;

            string statusFilter = request[this.FiltersSettings[ListTemplateFilter.DocumentStatuses].Name];
            int statusFilterID;
            bool statusesFiltered = Int32.TryParse(statusFilter, out statusFilterID) && statusFilterID != 0;

            string objectFilter = request[this.FiltersSettings[ListTemplateFilter.RelatedObjects].Name];
            Guid objectFilterID = Guid.Empty;
            bool objectsFiltered = Guid.TryParse(objectFilter, out objectFilterID) && objectFilterID != Guid.Empty;

            string setFilter = request[this.FiltersSettings[ListTemplateFilter.DocumentSets].Name];
            Guid setFilterID = Guid.Empty;
            bool setsFiltered = Guid.TryParse(setFilter, out setFilterID) && setFilterID != Guid.Empty;

            string sSearch = request["sSearch"];

            var documents = EntityContext.Current.Documents.Where(d =>
                d.OrganizationID == ParentID
                && (!typesFiltered || d.DocumentTypeID == typeFilterID)
                && (!statusesFiltered || d.DocumentStatusID == statusFilterID)
                && (!objectsFiltered || d.RelatedObjectID == objectFilterID)
                && (!setsFiltered || d.DocumentSets.Any(ds => ds.ID == setFilterID))
                && (canViewAllRelatedObjects || (d.RelatedObjectID != null && currentUser.AvailableRelatedObjects.Contains(d.RelatedObjectID.Value)))
                && (String.IsNullOrEmpty(sSearch) ||
                    d.Name.Contains(sSearch)
                    || d.Number.Contains(sSearch)
                    || d.DocumentType.Name.Contains(sSearch)
                    || d.DocumentStatu.Name.Contains(sSearch)
                    || d.RelatedObject.Name.Contains(sSearch)
                    || d.FileName.Contains(sSearch)
                    || d.Comment.Contains(sSearch)
                    || d.Tags.Contains(sSearch))
                ).Select(d =>
                    new
                    {
                        ID = d.ID,
                        Name = d.Name,
                        FileName = d.FileName,
                        Date = d.Date,
                        Expiration = d.ExpirationDate,
                        Status = d.DocumentStatu.Name,
                        Object = d.RelatedObject.Name
                    });
            
            totalRecords = documents.Count();

            var result = DataTableProcessor.ProcessTable(param, documents, out filteredRecords, Columns);

            return result.AsEnumerable().Select(d =>
                {
                    var item = (dynamic)d;
                    return new
                    {
                        ID = item.ID,
                        Name = item.Name,
                        FileName = item.FileName,
                        Date = item.Date.ToShortDateString(),
                        Expiration = item.Expiration != null ? item.Expiration.ToShortDateString() : null,
                        Status = item.Status,
                        Object = item.Object
                    };
                });
        }
    }
}
