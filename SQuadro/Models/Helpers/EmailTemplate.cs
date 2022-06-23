using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQuadro.Models
{
    public class EmailTemplate
    {
        private EmailTemplate()
        { }

        private EmailTemplate(int value, string text)
        {
            Value = value;
            Text = text;
            instance[value] = this;
        }

        private static readonly Dictionary<int, EmailTemplate> instance = new Dictionary<int, EmailTemplate>();

        public int Value { get; private set; }
        public string Text { get; private set; }

        public static explicit operator EmailTemplate(int value)
        {
            EmailTemplate result;
            if (instance.TryGetValue(value, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        public override String ToString()
        {
            return Text;
        }

        public static EmailTemplate InviteUser = new EmailTemplate(0, "Invite User");
        public static EmailTemplate ChangePassword = new EmailTemplate(1, "Change Password");
        public static EmailTemplate SendDocumentsAsAttachments = new EmailTemplate(2, "Send Documents as Attachments");
        public static EmailTemplate SendDocumentsAsLinks = new EmailTemplate(3, "Send Documents as Links");

        public static EmailTemplate[] EmailTemplates()
        {
            return new EmailTemplate[] { InviteUser, ChangePassword, SendDocumentsAsAttachments, SendDocumentsAsLinks };
        }
    }
}
