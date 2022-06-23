using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Models
{
    public class UsersList : BaseListTemplate
    {
        public UsersList(IUsersHelper IUsersHelper)
            : base("Users")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;
        private EntityContext context = EntityContext.Current;

        protected override void InitializeInternal()
        {
            Name = "Users";
            this.Readonly = currentUser.IsReadonly;

            JavaScriptFilePath = "~/Scripts/ListTemplate/usersList.js";
            JavaScriptClassName = "usersList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/Users/DataSourceCallback");

            if (!this.Readonly)
            {
                this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                        ListTemplateGlobalAction.AddNew));

                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "usersList.addNew()";
            }

            Columns = new List<Column>() {
                new Column() { Name = "ID", FilterType = FilterType.None }, 
                new Column() { Name = "Name", FilterType = FilterType.General }, 
                new Column() { Name = "Email", FilterType = FilterType.General },
                new Column() { Name = "SystemRole", FilterType = FilterType.None },
                new Column() { Name = "UserRole", FilterType = FilterType.General },
                new Column() { Name = "Actions", FilterType = FilterType.None }
            };

            DefaultSorting = new object[] { new object[] { 1, "asc" } };
        }

        public override object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords)
        {
            if (currentUser.Role != SystemRole.Admin.Value)
                throw new HttpException(403, "Access denied");

            var users = context.Users.Where(
                u => u.OrganizationID == currentUser.OrganizationID).Select(
                    u => new
                    {
                        ID = u.ID,
                        Name = u.Name,
                        Email = u.Email,
                        SystemRole = u.Role,
                        UserRole = u.UserRole.Name
                    });

            totalRecords = users.Count();

            var result = DataTableProcessor.ProcessTable(param, users.AsQueryable(), out filteredRecords, Columns);

            return result.AsEnumerable().Cast<dynamic>().Select(
                u => new
                {
                    ID = u.ID,
                    Name = u.Name,
                    Email = u.Email,
                    SystemRole = ((SystemRole)u.SystemRole).Text,
                    UserRole = u.UserRole
                });
        }
    }
}
