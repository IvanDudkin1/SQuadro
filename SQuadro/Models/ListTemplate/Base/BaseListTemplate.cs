using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SQuadro.Models
{
    public abstract class BaseListTemplate : IListTemplate
    {
        protected BaseListTemplate(string postfix)
        {
            this.Postfix = postfix;

            /*datetime filter settings*/
            dateFilterSettings = new DateFilterSettings()
            {
                Visible = false,
                NameFrom = "ltDateFilterFrom_{0}".ToFormat(postfix),
                NameTo = "ltDateFilterTo_{0}".ToFormat(postfix),
                CaptionFrom = "From:",
                CaptionTo = "To:"
            };

            /*upload file settings*/
            uploadFileSettings = new UploadFileSettings()
            {
                Visible = false
            };

            settings = new ListTemplateSettings()
            {
                AllowSelect = false,
                SuppressSelectionEvents = false
            };
        }

        private ListTemplateSettings settings;
        private DateFilterSettings dateFilterSettings;
        private UploadFileSettings uploadFileSettings;

        protected abstract void InitializeInternal();

        public void Initialize()
        {
            this.InitializeInternal();
        }

        public string Name { get; set; }

        public bool Readonly { get; set; }

        public List<Column> Columns { get; set; }

        public object DefaultSorting { get; set; }

        public object DefaultGrouping { get; set; }

        public ListTemplateSettings Settings
        {
            get { return settings; }
        }

        public DateFilterSettings DateFilterSettings { get { return dateFilterSettings; } }

        public UploadFileSettings UploadFileSettings { get { return uploadFileSettings; } }

        public Guid ParentID { get; set; }

        public Tuple<object, object, object> InitParams { get; set; }

        public string Postfix { get; protected set; }

        public string ReportName { get; set; }

        public string JavaScriptFilePath { get; set; }

        public string JavaScriptClassName { get; set; }

        public string DataSourceCallback { get; set; }

        public abstract object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords);

        public ListTemplateFiltersSettings FiltersSettings { get; protected set; }

        public ListTemplateGlobalActionsSettings GlobalActionsSettings { get; protected set; }

        public ListTemplateGlobalActionsSettings SelectionActionsSettings { get; protected set; }

        public string EditError { get; set; }

        public ActionResult Export(ExportType type)
        {
            throw new NotImplementedException();
        }
    }
}