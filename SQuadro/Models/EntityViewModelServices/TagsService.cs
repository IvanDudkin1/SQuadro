using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class DocumentTagsService
    {
        private static void UpdateTagFromModel(Tag tag, TagModel model, EntityContext context)
        {
            tag.OrganizationID = model.OrganizationID;
            tag.Name = model.Name;
        }

        public static TagModel GetViewModel(int? tagID, Guid organizationID, EntityContext context)
        {
            TagModel model = new TagModel() { OrganizationID = organizationID };
            if (tagID.HasValue && tagID != 0)
            {
                var tag = GetTag(tagID.Value, context);

                model.ID = tag.ID;
                model.OrganizationID = tag.OrganizationID;
                model.Name = tag.Name;
            }
            return model;
        }

        public static Tag SetTag(TagModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Tag tag = null;

            if (model.ID != 0)
            {
                tag = GetTag(model.ID, context);
            }
            else
            {
                tag = new Tag();
                context.Tags.AddObject(tag);
            }

            UpdateTagFromModel(tag, model, context);
            return tag;
        }

        public static Tag GetTag(Int64 id, EntityContext context)
        {
            var tag = context.Tags.SingleOrDefault(t => t.ID == id);

            if (tag == null)
                throw new InvalidOperationException("Tag with ID = {0} does not exist anymore".ToFormat(id));
            return tag;
        }

        public static void DeleteTag(int tagID, EntityContext context)
        {
            Tag tag = context.Tags.SingleOrDefault(t => t.ID == tagID);

            if (tag != null)
            {
                if (context.Documents.Any(d => d.OrganizationID == tag.OrganizationID && d.Tags.Contains(tag.Name)))
                    throw new InvalidOperationException("There are Documents with this Tag. Deletion aborted");

                context.Tags.DeleteObject(tag);
            }
        }

        public static Tag AddNew(string name, Guid organizationID, EntityContext context)
        {
            Tag tag = new Tag() { OrganizationID = organizationID, Name = name };
            context.Tags.AddObject(tag);
            return tag;
        }
    }
}