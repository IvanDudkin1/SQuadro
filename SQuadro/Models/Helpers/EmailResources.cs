using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SQuadro.Models
{
    public class EmailDefinition
    {
        public EmailDefinition(Guid organizationID, EmailTemplate template, IDictionary<string, string> parameters)
        {
            var emailTemplate = EmailTemplatesService.GetEmailTemplate(template, organizationID, EntityContext.Current);
            Subject = emailTemplate.Subject;
            SetBody(emailTemplate, parameters);
        }

        private string body = "";
        private bool isHtml = true;
        private List<string> emailTo = new List<string>();
        private List<CustomAttachment> attachments = new List<CustomAttachment>();

        private void SetBody(EmailTemplates template, IDictionary<string, string> parameters)
        {
            body = "<p>{0}</p><p>{1}</p><p>{2}</p>".ToFormat(template.Salutation, template.Body, template.Signature);
            foreach (var kvp in parameters)
                body = body.Replace("{{" + kvp.Key + "}}", kvp.Value);
        }
        
        public string Subject { get; set; }
        public string Body { get { return body; } }
        public bool IsHtml { get { return isHtml; } set { isHtml = value; } }
        public EmailFrom EmailFrom { get; set; }
        public IList<string> EmailTo { get {return emailTo; } }
        public IList<CustomAttachment> Attachments { get { return attachments; } }

        private string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        public string SmtpServer { get { return smtpServer; } set { smtpServer = value; } }

        private int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
        public int SmtpPort { get { return smtpPort; } set { smtpPort = value; } }

        private string smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
        public string SmtpUser { get { return smtpUser; } set { smtpUser = value; } }

        private string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
        public string SmtpPassword { get { return smtpPassword; } set { smtpPassword = value; } }

        private bool enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
        public bool EnableSsl { get { return enableSsl; } set { enableSsl = value; } }

        public void AddAttachments(CustomAttachment[] attachments)
        {
            this.attachments.AddRange(attachments);
        }

        public void AddRecipients(string[] recipients)
        {
            this.emailTo.AddRange(recipients);
        }
    }

    public class CustomAttachment
    {
        public CustomAttachment(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }

        public string FileName { get; private set; }
        public byte[] Content { get; private set; }
    }

    public struct EmailFrom
    {
        public EmailFrom(string name, string email)
            : this()
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
    }
}