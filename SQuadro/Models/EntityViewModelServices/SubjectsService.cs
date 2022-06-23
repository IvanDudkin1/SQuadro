using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class SubjectsService
    {
        private static void UpdateSubjectFromModel(Subject subject, SubjectModel model, EntityContext context)
        {
            subject.OrganizationID = model.OrganizationID;
            subject.Text = model.Text;
        }

        public static SubjectModel GetViewModel(int? subjectID, Guid organizationID, EntityContext context)
        {
            SubjectModel model = new SubjectModel() { OrganizationID = organizationID };
            if (subjectID.HasValue && subjectID != 0)
            {
                var subject = GetSubject(subjectID.Value, context);

                model.ID = subject.ID;
                model.OrganizationID = subject.OrganizationID;
                model.Text = subject.Text;
            }
            return model;
        }

        public static Subject SetSubject(SubjectModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Subject subject = null;

            if (model.ID != 0)
            {
                subject = GetSubject(model.ID, context);
            }
            else
            {
                subject = new Subject();
                context.Subjects.AddObject(subject);
            }

            UpdateSubjectFromModel(subject, model, context);
            return subject;
        }

        public static Subject GetSubject(Int64 id, EntityContext context)
        {
            var subject = context.Subjects.SingleOrDefault(t => t.ID == id);

            if (subject == null)
                throw new InvalidOperationException("Subject with ID = {0} does not exist anymore".ToFormat(id));
            return subject;
        }

        public static void DeleteSubject(int subjectID, EntityContext context)
        {
            Subject subject = context.Subjects.SingleOrDefault(t => t.ID == subjectID);

            if (subject != null)
            {
                context.Subjects.DeleteObject(subject);
            }
        }

        public static Subject AddNew(string text, Guid organizationID, EntityContext context)
        {
            Subject subject = new Subject() { OrganizationID = organizationID, Text = text };
            context.Subjects.AddObject(subject);
            return subject;
        }
    }
}