using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SQuadro.Models
{
    public interface IListTemplate
    {
        string Name { get; set; }
        void Initialize();
        bool Readonly { get; set; }
        Guid ParentID { get; set; }
        Tuple<object, object, object> InitParams { get; set; }
        string Postfix { get; }
        string JavaScriptFilePath { get; }
        string JavaScriptClassName { get; }
        string DataSourceCallback { get; }
        string ReportName { get; }
        List<Column> Columns { get; set; }
        object DefaultSorting { get; set; } // array [[iColumnIndex, sSortType]] where sSortType is 'asc' or 'desc'
        object DefaultGrouping { get; set; } // array [[iColumnIndex, sSortType]]
        ListTemplateFiltersSettings FiltersSettings { get; }
        ListTemplateGlobalActionsSettings GlobalActionsSettings { get; }
        ListTemplateGlobalActionsSettings SelectionActionsSettings { get; }
        DateFilterSettings DateFilterSettings { get; }
        UploadFileSettings UploadFileSettings { get; } 
        string EditError { get; set; }
        object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords);
        ActionResult Export(ExportType exportType);
        ListTemplateSettings Settings { get; }
    }
}