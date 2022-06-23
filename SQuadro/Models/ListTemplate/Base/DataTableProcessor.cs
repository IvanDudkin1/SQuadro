using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace SQuadro.Models
{
    public class DataTableProcessor
    {
        public static IQueryable<object> ProcessTable(DataTablesParam param, IQueryable data, out int totalRecordsDisplay, List<Column> columns)
        {
            if (!String.IsNullOrEmpty(param.sSearch))
            {
                string searchString = "";
                int paramsCounter = 0;
                List<object> parameters = new List<object>();
                bool first = true;
                for (int i = 0; i < param.iColumns; i++)
                {
                    if (param.bSearchable[i] && columns[i].FilterType != FilterType.None)
                    {
                        string columnName = columns[i].Name;

                        Action CheckFirst = () => 
                            {
                                if (!first)
                                    searchString += " or ";
                                else
                                    first = false;
                            };

                        if (columns[i].FilterType == FilterType.Numeric)
                        {
                            decimal value;
                            if (Decimal.TryParse(param.sSearch, out value))
                            {
                                CheckFirst();
                                searchString += "{0} == @{1}".ToFormat(columnName, paramsCounter);
                                paramsCounter++;
                                parameters.Add(value);
                            }
                        }
                        else if (columns[i].FilterType == FilterType.Date)
                        {
                            DateTime value;
                            if (DateTime.TryParse(param.sSearch, out value))
                            {
                                CheckFirst();
                                searchString += "{0} == @{1}".ToFormat(columnName, paramsCounter);
                                paramsCounter++;
                                parameters.Add(value);
                            }
                        }
                        else
                        {
                            CheckFirst();
                            searchString += columnName + ".ToLower().Contains(\"" + param.sSearch + "\".ToLower())";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(searchString))
                    data = data.Where(searchString, parameters.ToArray());
            }
            string sortString = "";
            for (int i = 0; i < param.iSortingCols; i++)
            {
                int columnNumber = param.iSortCol[i];
                string columnName = columns[columnNumber].Name;
                string sortDir = param.sSortDir[i];
                if (i != 0)
                    sortString += ", ";
                sortString += columnName + " " + sortDir;
            }

            totalRecordsDisplay = data.Count();

            if (!String.IsNullOrEmpty(sortString))
                data = data.OrderBy(sortString);
            if (param.iDisplayLength > 0)
                data = data.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            return (IQueryable<object>)data;
        }
    }
}