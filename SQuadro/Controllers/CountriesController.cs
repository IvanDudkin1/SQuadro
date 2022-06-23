using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;

namespace SQuadro.Controllers
{
    [Authorize]
    public class CountriesController : Controller
    {
        [HttpPost]
        public ActionResult GetList(string term)
        {
            return Json(ListsHelper.Countries().Where(c => String.IsNullOrEmpty(term) || c.Name.ToLower().Contains(term.ToLower()) || c.ID_Alpha2.ToLower().Contains(term.ToLower())).Select(
                c => new Select2ListItem() { id = c.ID_Alpha2, text = c.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedItem(string selection)
        {
            string result = String.Empty;
            Country country = EntityContext.Current.Countries.FirstOrDefault(c => c.ID_Alpha2 == selection);
            if (country != null)
                result = country.Name;
            else
                result = "Country with ID {0} does not exist anymore".ToFormat(selection);

            return Json(new { id = selection, text = result });
        }
    }
}
