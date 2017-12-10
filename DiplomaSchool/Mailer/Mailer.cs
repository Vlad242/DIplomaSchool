using System;
using System.Net;
using System.Net.Mail;

namespace DiplomaSchool.Mailer
{
    class Mailer
    {
        public void SendMail(string toList, string from, string ccList, string subject, string body, string attachFile = null)
        {
            MailerConfig mailer = new MailerConfig();
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                MailAddress fromAddress = new MailAddress(from);
                message.From = fromAddress;
                message.To.Add(toList);
                if (ccList != null && ccList != string.Empty)
                    message.CC.Add(ccList);
                if (!string.IsNullOrEmpty(attachFile))
                    message.Attachments.Add(new Attachment(attachFile));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                // We use gmail as our smtp client
                smtpClient.Host = mailer.host;
                smtpClient.Port = mailer.port;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(
                    mailer.userName, mailer.password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
            catch (Exception)
            {
            }
        }
    }
}
