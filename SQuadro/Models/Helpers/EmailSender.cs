using System;
using System.Linq;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;
using System.Net.Mime;
using System.Web;
using System.IO;

namespace SQuadro.Models
{
    public static class EmailSender
    {
        public static void SendEmail(EmailDefinition emailDefinition)
        {
            if (emailDefinition.EmailTo == null || !emailDefinition.EmailTo.Any()) return;

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(emailDefinition.EmailFrom.Email, emailDefinition.EmailFrom.Name, Encoding.UTF8);
                message.Sender = new MailAddress(emailDefinition.EmailFrom.Email, emailDefinition.EmailFrom.Name, Encoding.UTF8);
                message.ReplyToList.Add(new MailAddress(emailDefinition.EmailFrom.Email));
                foreach (string email in emailDefinition.EmailTo)
                {
                    message.To.Add(new MailAddress(email, email, Encoding.UTF8));
                }
                message.HeadersEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                message.Subject = emailDefinition.Subject;
                message.Body = emailDefinition.Body;
                message.IsBodyHtml = emailDefinition.IsHtml;

                if (emailDefinition.Attachments != null)
                    foreach (CustomAttachment attach in emailDefinition.Attachments)
                    {
                        message.Attachments.Add(new Attachment(new MemoryStream(attach.Content), attach.FileName));
                    }

                using (SmtpClient smtpClient = new SmtpClient(emailDefinition.SmtpServer, emailDefinition.SmtpPort))
                {
                    if (!String.IsNullOrEmpty(emailDefinition.SmtpUser))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(emailDefinition.SmtpUser, emailDefinition.SmtpPassword);
                    }
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = emailDefinition.EnableSsl;
                    ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    smtpClient.Send(message);
                }
            }
        }
    }

    public class AttachmentHelper
    {
        public static Attachment CreateAttachment(string attachmentFile, string displayName, TransferEncoding transferEncoding)
        {
            Attachment attachment = new Attachment(attachmentFile);
            attachment.TransferEncoding = transferEncoding;

            string tranferEncodingMarker = String.Empty;
            string encodingMarker = String.Empty;
            int maxChunkLength = 0;

            switch (transferEncoding)
            {
                case TransferEncoding.Base64:
                    tranferEncodingMarker = "B";
                    encodingMarker = "UTF-8";
                    maxChunkLength = 30;
                    break;
                case TransferEncoding.QuotedPrintable:
                    tranferEncodingMarker = "Q";
                    encodingMarker = "ISO-8859-1";
                    maxChunkLength = 76;
                    break;
                default:
                    throw (new ArgumentException(String.Format("The specified TransferEncoding is not supported: {0}", transferEncoding, "transferEncoding")));
            }

            attachment.NameEncoding = Encoding.GetEncoding(encodingMarker);

            string encodingtoken = String.Format("=?{0}?{1}?", encodingMarker, tranferEncodingMarker);
            string softbreak = "?=";
            string encodedAttachmentName = encodingtoken;

            if (attachment.TransferEncoding == TransferEncoding.QuotedPrintable)
                encodedAttachmentName = HttpUtility.UrlEncode(displayName, Encoding.Default).Replace("+", " ").Replace("%", "=");
            else
                encodedAttachmentName = Convert.ToBase64String(Encoding.UTF8.GetBytes(displayName));

            encodedAttachmentName = SplitEncodedAttachmentName(encodingtoken, softbreak, maxChunkLength, encodedAttachmentName);
            attachment.Name = encodedAttachmentName;

            return attachment;
        }

        private static string SplitEncodedAttachmentName(string encodingtoken, string softbreak, int maxChunkLength, string encoded)
        {
            int splitLength = maxChunkLength - encodingtoken.Length - (softbreak.Length * 2);
            var parts = SplitByLength(encoded, splitLength);

            string encodedAttachmentName = encodingtoken;

            foreach (var part in parts)
                encodedAttachmentName += part + softbreak + encodingtoken;

            encodedAttachmentName = encodedAttachmentName.Remove(encodedAttachmentName.Length - encodingtoken.Length, encodingtoken.Length);
            return encodedAttachmentName;
        }

        private static IEnumerable<string> SplitByLength(string stringToSplit, int length)
        {
            while (stringToSplit.Length > length)
            {
                yield return stringToSplit.Substring(0, length);
                stringToSplit = stringToSplit.Substring(length);
            }

            if (stringToSplit.Length > 0) yield return stringToSplit;
        }
    }
}