using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class VesselsService
    {
        private static void UpdateVesselFromModel(Vessel vessel, VesselModel model, User currentUser, EntityContext context)
        {
            if (!currentUser.CanAddRelatedObject && !currentUser.AvailableCategories.Contains(vessel.ID))
                throw new UserException("Modifying is forbidden");

            vessel.OrganizationID = model.OrganizationID;
            vessel.Name = model.Name;
        }

        public static VesselModel GetViewModel(Guid? vesselID, Guid organizationID, EntityContext context)
        {
            VesselModel model = new VesselModel() { OrganizationID = organizationID };
            if (vesselID != null && vesselID != Guid.Empty)
            {
                var vessel = context.RelatedObjects.OfType<Vessel>().FirstOrDefault(c => c.ID == vesselID);
                if (vessel == null)
                    throw new InvalidOperationException("Vessel with ID = {0} does not exist anymore".ToFormat(vesselID));

                model.ID = vessel.ID;
                model.Name = vessel.Name;
            }
            return model;
        }

        public static Vessel SetVessel(VesselModel model, User currentUser, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Vessel vessel = null;

            if (model.ID != Guid.Empty)
            {
                vessel = context.RelatedObjects.OfType<Vessel>().FirstOrDefault(c => c.ID == model.ID);

                if (vessel == null)
                    throw new InvalidOperationException("Vessel with ID = {0} does not exist anymore".ToFormat(model.ID));
            }
            else
            {
                vessel = new Vessel();
                context.RelatedObjects.AddObject(vessel);
            }

            UpdateVesselFromModel(vessel, model, currentUser, context);
            return vessel;
        }

        public static void DeleteVessel(Guid vesselID, User currentUser, EntityContext context)
        {
            if (!currentUser.CanAddRelatedObject)
                throw new UserException("Deletion is forbidden");

            var vessel = context.RelatedObjects.FirstOrDefault(c => c.ID == vesselID);
            if (vessel.Documents.Any())
                throw new InvalidOperationException("There are Documents with this Related Object. Deletion aborted");

            if (vessel != null)
            {
                context.RelatedObjects.DeleteObject(vessel);
            }
        }

        public static Vessel AddNew(string name, User currentUser, EntityContext context)
        {
            if (!currentUser.CanAddCategory)
                throw new UserException("Adding new vessels is forbidden.");

            Vessel vessel = new Vessel() { OrganizationID = currentUser.OrganizationID, Name = name };
            context.RelatedObjects.AddObject(vessel);
            return vessel;
        }
    }
}