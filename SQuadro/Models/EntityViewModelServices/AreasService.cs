using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class AreasService
    {
        private static void UpdateAreaFromModel(Area area, AreaModel model, EntityContext context)
        {
            area.OrganizationID = model.OrganizationID;
            area.Name = model.Name;
        }

        public static AreaModel GetViewModel(Guid? areaID, Guid organizationID, EntityContext context)
        {
            AreaModel model = new AreaModel() { OrganizationID = organizationID };
            if (areaID != null && areaID != Guid.Empty)
            {
                var area = context.Areas.FirstOrDefault(a => a.ID == areaID);
                if (area == null)
                    throw new InvalidOperationException("Area with ID = {0} does not exist anymore".ToFormat(areaID));

                model.ID = area.ID;
                model.OrganizationID = area.OrganizationID;
                model.Name = area.Name;
            }
            return model;
        }

        public static Area SetArea(AreaModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Area area = null;

            if (model.ID != Guid.Empty)
            {
                area = context.Areas.FirstOrDefault(c => c.ID == model.ID);

                if (area == null)
                    throw new InvalidOperationException("Area with ID = {0} does not exist anymore".ToFormat(model.ID));
            }
            else
            {
                area = new Area();
                context.Areas.AddObject(area);
            }

            UpdateAreaFromModel(area, model, context);
            return area;
        }

        public static void DeleteArea(Guid areaID, EntityContext context)
        {
            Area area = context.Areas.FirstOrDefault(c => c.ID == areaID);
            if (area.CompanyAreas.Any())
                throw new InvalidOperationException("There are Partners with this Area. Deletion aborted");

            if (area != null)
            {
                context.Areas.DeleteObject(area);
            }
        }

        public static Area AddNew(string name, Guid organizationID, EntityContext context)
        {
            Area area = new Area() { OrganizationID = organizationID, Name = name };
            context.Areas.AddObject(area);
            return area;
        }
    }
}