using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class EmailTemplatesList : BaseListTemplate
    {
        public EmailTemplatesList(IUsersHelper IUsersHelper)
            : base("EmailTemplates")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            JavaScriptFilePath = "~/Scripts/ListTemplate/emailTemplatesList.js";
            JavaScriptClassName = "emailTemplatesList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/EmailTemplates/DataSourceCallback");

            this.Name = "Email Templates";
            this.Readonly = currentUser.IsReadonly;

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "emailTemplatesList.addNew()";
            }

            Columns = new List<Column>() {
                new Column() { Name = "ID", FilterType = FilterType.None },
                new Column() { Name = "Type", FilterType = FilterType.None },
                new Column() { Name = "Subject", FilterType = FilterType.General },
                new Column() { Name = "Salutation", FilterType = FilterType.General },
                new Column() { Name = "Body", FilterType = FilterType.General },
                new Column() { Name = "Signature", FilterType = FilterType.General },
                new Column() { Name = "Actions", FilterType = FilterType.None },
            };

            DefaultSorting = new object[] { new object[] { 1, "asc" }, new object[] { 4, "desc" }, new object[] { 3, "asc" } };
        }

        public override object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords)
        {
            var emailTemplates = EntityContext.Current.EmailTemplates.Where(t => t.OrganizationID == ParentID).Select(t =>
                new
                {
                    ID = t.ID,
                    Type = t.Type,
                    Subject = t.Subject,
                    Salutation = t.Salutation,
                    Body = t.Body,
                    Signature = t.Signature
                });

            totalRecords = emailTemplates.Count();

            var result = DataTableProcessor.ProcessTable(param, emailTemplates, out filteredRecords, Columns);

            return result.AsEnumerable().Select(i =>
                {
                    var item = (dynamic)i;
                    string body = item.Body;
                    return new
                    {
                        ID = item.ID,
                        Type = ((EmailTemplate)item.Type).Text,
                        Subject = item.Subject,
                        Salutation = item.Salutation,
                        Body = body.Left(50) + (body.Length > 50 ? "..." : ""),
                        Signature = item.Signature
                    };
                });
        }
    }
}
