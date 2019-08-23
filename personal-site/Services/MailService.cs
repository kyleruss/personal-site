using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services
{
    public class MailService
    {
        private static MailService _instance;

        private MailService() { }

        public async Task<bool> SendMessage(MailMessage message)
        {
            NameValueCollection config = ConfigurationManager.AppSettings;

            try
            {
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = config.Get("mailHost"),
                    Port = int.Parse(config.Get("mailPort")),
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(config.Get("mailAddress"), config.Get("mailPassword"))
                };

                await smtpClient.SendMailAsync(message);
                return true;
            }

            catch(SmtpException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private MailMessage PrepareMessage(string title, string body)
        {
            NameValueCollection config = ConfigurationManager.AppSettings;

            return new MailMessage(config.Get("mailAddress"), config.Get("targetAddress"), title, body);
        }

        public MailMessage PrepareContactMessage(ContactViewModel contactViewModel)
        {
            string messageBody = string.Format("Name: {0}\nEmail: {1}\nMessage: \n{2}", 
                                 contactViewModel.ContactName, contactViewModel.ContactEmail, contactViewModel.ContactMessage);
            string messageTitle = "CONTACT MESSAGE";

            return PrepareMessage(messageTitle, messageBody);
        }

        public MailMessage PrepareAuthCodeMessage(string code)
        {
            string date = DateTime.Now.ToString();
            string messageBody = string.Format("Date: {0}\nCode: {1}", date, code);
            string messageTitle = "SECURITY CODE";

            return PrepareMessage(messageTitle, messageBody);
        }

        public static MailService GetInstance()
        {
            _instance = _instance ?? new MailService();
            return _instance;
        }
    }
}