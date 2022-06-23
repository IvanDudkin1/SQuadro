using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class EmailTemplatesService
    {
        private static void UpdateEmailTemplateFromModel(EmailTemplates emailTemplates, EmailTemplatesModel model, EntityContext context)
        {
            emailTemplates.OrganizationID = model.OrganizationID;
            emailTemplates.Type = model.Type;
            emailTemplates.Subject = model.Subject;
            emailTemplates.Salutation = model.Salutation;
            emailTemplates.Body = model.Body;
            emailTemplates.Signature = model.Signature;
        }

        public static EmailTemplates GetEmailTemplate(Guid id, EntityContext context)
        {
            var emailTemplate = context.EmailTemplates.SingleOrDefault(t => t.ID == id);
            if (emailTemplate == null)
                throw new InvalidOperationException("EmailTemplate with ID = {0} does not exist anymore".ToFormat(id));
            return emailTemplate;
        }

        public static EmailTemplates GetEmailTemplate(EmailTemplate template, Guid organizationID, EntityContext context)
        {
            var emailTemplate = context.EmailTemplates
                .SingleOrDefault(t => t.OrganizationID == organizationID && t.Type == template.Value);

            if (emailTemplate == null)
                throw new UserException("Email Template {0} was not defined. Contact to Administrator.".ToFormat(template.Text));

            return emailTemplate;
        }

        public static EmailTemplatesModel GetViewModel(Guid? emailTemplateID, Guid organizationID, EntityContext context)
        {
            EmailTemplatesModel model = new EmailTemplatesModel() { OrganizationID = organizationID };
            if (emailTemplateID != null && emailTemplateID != Guid.Empty)
            {
                var emailTemplates = GetEmailTemplate(emailTemplateID.Value, context);

                model.ID = emailTemplates.ID;
                model.OrganizationID = emailTemplates.OrganizationID;
                model.Type = emailTemplates.Type;
                model.Subject = emailTemplates.Subject;
                model.Salutation = emailTemplates.Salutation;
                model.Body = emailTemplates.Body;
                model.Signature = emailTemplates.Signature;
            }
            return model;
        }

        public static EmailTemplates SetEmailTemplate(EmailTemplatesModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            EmailTemplates template = null;

            if (model.ID != Guid.Empty)
            {
                template = GetEmailTemplate(model.ID, context);
            }
            else
            {
                template = new EmailTemplates();
                context.EmailTemplates.AddObject(template);
            }

            UpdateEmailTemplateFromModel(template, model, context);
            return template;
        }

        public static void DeleteEmailTemplate(Guid id, EntityContext context)
        {
            EmailTemplates template = context.EmailTemplates.SingleOrDefault(t => t.ID == id);
            if (template != null)
            {
                context.EmailTemplates.DeleteObject(template);
            }
        }
    }
}