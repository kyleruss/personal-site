using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace personal_site.Services
{
    public class ContactService
    {
        private static ContactService _instance;

        private ContactService() { }

        public void SendMessage(string contactName, string contactEmail, string contactMessage)
        {
            NameValueCollection config = ConfigurationManager.AppSettings;

            using(SmtpClient smtpClient = new SmtpClient(config.Get("mailHost"), int.Parse(config.Get("mailPort"))))
            {
                smtpClient.Host         =   config.Get("mailHost");
                smtpClient.Port         =   int.Parse(config.Get("mailPort"));
                smtpClient.Credentials  =   new NetworkCredential(config.Get("mailAddress"), config.Get("mailPassword"));
                smtpClient.EnableSsl    =   true;

                MailMessage message = PrepareMessage(contactName, contactEmail, contactMessage, config);
                smtpClient.Send(message);
            }
        
        }

        private MailMessage PrepareMessage(string contactName, string contactEmail, string message, NameValueCollection config)
        {
            string messageBody  = string.Format("Name: {0}\nEmail: {1}\nMessage: \n{3}", contactName, contactEmail, message);
            string messageTitle = "CONTACT MESSAGE";

            return new MailMessage(config.Get("mailAddress"), config.Get("targetAddress"), messageTitle, messageBody);
        }

        public static ContactService GetInstance()
        {
            _instance = _instance ?? new ContactService();
            return _instance;
        }
    }
}