using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class EmailSettingsService
    {
        private static void UpdateEmailSettingsFromModel(EmailSettings emailSettings, EmailSettingsModel model, EntityContext context)
        {
            emailSettings.OrganizationID = model.OrganizationID;
            emailSettings.Name = model.Name;
            emailSettings.Email = model.Email;
            emailSettings.IsDefault = model.IsDefault;
            emailSettings.SmtpServer = model.SmtpServer;
            emailSettings.SmtpPort = model.SmtpPort;
            emailSettings.SmtpUser = model.SmtpUser;
            emailSettings.SmtpPassword = SecurityHelper.Encrypt(model.SmtpPassword);
            emailSettings.EnableSsl = model.EnableSsl;
        }

        public static EmailSettings GetEmailSettings(Guid id, EntityContext context)
        {
            var emailSettings = context.EmailSettings.SingleOrDefault(s => s.ID == id);
            if (emailSettings == null)
                throw new InvalidOperationException("EmailSettings with ID = {0} does not exist anymore".ToFormat(id));
            return emailSettings;
        }

        public static EmailSettingsModel GetViewModel(Guid? emailSettingsID, Guid organizationID, EntityContext context)
        {
            EmailSettingsModel model = new EmailSettingsModel() { OrganizationID = organizationID };
            if (emailSettingsID != null && emailSettingsID != Guid.Empty)
            {
                var emailSettings = GetEmailSettings(emailSettingsID.Value, context);

                model.ID = emailSettings.ID;
                model.OrganizationID = emailSettings.OrganizationID;
                model.Name = emailSettings.Name;
                model.Email = emailSettings.Email;
                model.IsDefault = emailSettings.IsDefault;
                model.SmtpServer = emailSettings.SmtpServer;
                model.SmtpPort = emailSettings.SmtpPort;
                model.SmtpUser = emailSettings.SmtpUser;
                model.SmtpPassword = SecurityHelper.Decrypt(emailSettings.SmtpPassword);
                model.EnableSsl = emailSettings.EnableSsl;
            }
            return model;
        }

        public static EmailSettings SetEmailSettings(EmailSettingsModel model, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            EmailSettings settings = null;

            if (model.ID != Guid.Empty)
            {
                settings = GetEmailSettings(model.ID, context);
            }
            else
            {
                settings = new EmailSettings();
                context.EmailSettings.AddObject(settings);
            }

            UpdateEmailSettingsFromModel(settings, model, context);
            return settings;
        }

        public static void DeleteEmailSettings(Guid id, EntityContext context)
        {
            EmailSettings settings = context.EmailSettings.SingleOrDefault(s => s.ID == id);
            if (settings != null)
            {
                context.EmailSettings.DeleteObject(settings);
            }
        }
    }
}