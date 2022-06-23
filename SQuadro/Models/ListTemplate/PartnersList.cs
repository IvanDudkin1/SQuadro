using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class PartnersList : BaseListTemplate
    {
        public PartnersList(IUsersHelper IUsersHelper)
            : base("Partners")
        {
            currentUser = IUsersHelper.CurrentUser;
        }

        private User currentUser;

        protected override void InitializeInternal()
        {
            bool isSelectionList = false;
            if (this.InitParams != null)
                isSelectionList = (bool)this.InitParams.Item1;

            if (!isSelectionList)
                Name = "Companies";

            this.Readonly = currentUser.IsReadonly;

            JavaScriptFilePath = "~/Scripts/ListTemplate/{0}".ToFormat(isSelectionList ? "partnersSelectionList.js" : "partnersList.js");
            JavaScriptClassName = "partnersList";
            DataSourceCallback = VirtualPathUtility.ToAbsolute("~/Partners/DataSourceCallback");

            this.FiltersSettings = new ListTemplateFiltersSettings(
                new ListTemplateFilterProperties(this.Postfix,
                    ListTemplateFilter.PartnerCategories,
                    ListTemplateFilter.PartnerAreas,
                    ListTemplateFilter.Countries));

            this.FiltersSettings[ListTemplateFilter.PartnerCategories].DataSource = ListsHelper.PartnerCategoriesList(currentUser, addAll: true);
            this.FiltersSettings[ListTemplateFilter.PartnerCategories].SelectedValue = Guid.Empty.ToString();

            this.FiltersSettings[ListTemplateFilter.PartnerAreas].DataSource = ListsHelper.PartnerAreasList(ParentID, addAll: true);
            this.FiltersSettings[ListTemplateFilter.PartnerAreas].SelectedValue = Guid.Empty.ToString();

            this.FiltersSettings[ListTemplateFilter.Countries].DataSource = ListsHelper.CountriesList(addAll: true);
            this.FiltersSettings[ListTemplateFilter.Countries].SelectedValue = String.Empty;

            if (!isSelectionList)
            {
                if (!this.Readonly)
                {
                    this.GlobalActionsSettings = new ListTemplateGlobalActionsSettings(
                        new ListTemplateGlobalActionProperties(this.Postfix,
                            ListTemplateGlobalAction.AddNew));

                    this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Text = "Add New";
                    this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-plus\"></i>";
                    this.GlobalActionsSettings[ListTemplateGlobalAction.AddNew].ButtonSettings.Click = "partnersList.addNew()";
                }

                this.SelectionActionsSettings = new ListTemplateGlobalActionsSettings(
                    new ListTemplateGlobalActionProperties(this.Postfix,
                    ListTemplateGlobalAction.Email));

                this.SelectionActionsSettings[ListTemplateGlobalAction.Email].ButtonSettings.Text = "Send Documents";
                this.SelectionActionsSettings[ListTemplateGlobalAction.Email].ButtonSettings.Html = "<i class=\"glyphicon glyphicon-envelope\"></i>";
                this.SelectionActionsSettings[ListTemplateGlobalAction.Email].ButtonSettings.Click = "partnersList.email()";

            }

            this.Settings.AllowSelect = !this.Readonly;
            this.Settings.SuppressSelectionEvents = isSelectionList;

            Columns = new List<Column>() {
                new Column() { Name = "Selector", FilterType = FilterType.None, IsSelector = true }, 
                new Column() { Name = "ID", FilterType = FilterType.None }, 
                new Column() { Name = "Name", FilterType = FilterType.None }, 
                new Column() { Name = "FullName", FilterType = FilterType.None },
                new Column() { Name = "Contacts", FilterType = FilterType.None },
                new Column() { Name = "Country", FilterType = FilterType.None },
                new Column() { Name = "Categories", FilterType = FilterType.None },
                new Column() { Name = "Areas", FilterType = FilterType.None },
                new Column() { Name = "Actions", FilterType = FilterType.None }
            };

            DefaultSorting = new object[] { new object[] { 2, "asc" } };
        }

        public override object GetDataSource(DataTablesParam param, HttpRequestBase request, out int totalRecords, out int filteredRecords)
        {
            bool canViewAllCategories = currentUser.CanAddCategory;

            string commonFilter = param.sSearch;
            bool checkCommonFilter = !String.IsNullOrEmpty(commonFilter);

            string categoryFilter = request[this.FiltersSettings[ListTemplateFilter.PartnerCategories].Name];
            Guid categoryFilterID = Guid.Empty;
            bool categoriesFiltered = Guid.TryParse(categoryFilter, out categoryFilterID);

            string areaFilter = request[this.FiltersSettings[ListTemplateFilter.PartnerAreas].Name];
            Guid areaFilterID = Guid.Empty;
            bool areasFiltered = Guid.TryParse(areaFilter, out areaFilterID);

            string countryFilter = request[this.FiltersSettings[ListTemplateFilter.Countries].Name];
            bool countriesFiltered = !String.IsNullOrEmpty(countryFilter);

            var partners = EntityContext.Current.Companies.Where(c =>
                c.OrganizationID == ParentID
                && (!categoriesFiltered || c.CompanyCategories.Any(cc => cc.CategoryID == categoryFilterID))
                && (!areasFiltered || c.CompanyAreas.Any(ca => ca.AreaID == areaFilterID))
                && (!countriesFiltered || c.CountryID_Alpha2 == countryFilter)
                && (canViewAllCategories || c.CompanyCategories.Any(cc => currentUser.AvailableCategories.Contains(cc.CategoryID))));
                
            
            totalRecords = partners.Count();

            var result = partners.Where(
                c=> !checkCommonFilter
                    || c.Name.Contains(commonFilter)
                    || c.FullName.Contains(commonFilter)
                    || c.Comment.Contains(commonFilter)
                    || c.Address.Contains(commonFilter)
                    || c.CompanyCategories.Any(cc => cc.Category.Name.Contains(commonFilter))
                    || c.CompanyAreas.Any(ca => ca.Area.Name.Contains(commonFilter))
                    || c.Contacts.Any(ca => ca.Data.Contains(commonFilter) || ca.Comment.Contains(commonFilter))
                    || c.Country.Name.Contains(commonFilter)).Select(c =>
                    new
                    {
                        ID = c.ID,
                        Name = c.Name,
                        FullName = c.FullName,
                        Categories = c.CompanyCategories.Select(cc => new { ID = cc.CategoryID, Name = cc.Category.Name }),
                        Areas = c.CompanyAreas.Select(ca => new { ID = ca.AreaID, Name = ca.Area.Name }),
                        Contacts = c.Contacts.Where(cn => cn.IsPrimary).OrderBy(cn => cn.ContactTypeID).Select(cn => new { ID = cn.ContactTypeID, Name = cn.Data, Type = cn.ContactType.Name }),
                        Country = c.Country.Name
                    });

            return DataTableProcessor.ProcessTable(param, result, out filteredRecords, Columns);
        }
    }
}
