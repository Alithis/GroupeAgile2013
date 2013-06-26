using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;

namespace NoteTaLoc.Utilitary
{
    public class MailControler
    {
        private readonly Configuration config;
        private readonly SmtpClient smtpClient;
        private String adminMail;

        public MailControler()
        {
            config = WebConfigurationManager.OpenWebConfiguration("~");

            smtpClient = new SmtpClient
                {
                    Host = config.AppSettings.Settings["ServerMail"].Value,
                    Port = int.Parse(config.AppSettings.Settings["ServerMailPort"].Value),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials =
                        new NetworkCredential(config.AppSettings.Settings["adminMailUID"].Value,
                                              config.AppSettings.Settings["adminMailPWD"].Value)
                };

            adminMail = config.AppSettings.Settings["adminMail"].Value;
        }

        public void sendEmail(String sender, String dest, String message, String subject)
        {
            var mail = new MailMessage();
            mail.To.Add(dest);
            mail.Subject = subject;
            mail.From = new MailAddress(sender);
            mail.Body = message;
            smtpClient.Send(mail);
        }
    }
}