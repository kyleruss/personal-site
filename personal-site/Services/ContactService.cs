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
    public class ContactService
    {
        private static ContactService _instance;

        private ContactService() { }

        public async Task SendMessage(ContactViewModel contactViewModel)
        {
            NameValueCollection config = ConfigurationManager.AppSettings;

            try
            {
                SmtpClient smtpClient = new SmtpClient();

                smtpClient.Host = config.Get("mailHost");
                smtpClient.Port = int.Parse(config.Get("mailPort"));
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(config.Get("mailAddress"), config.Get("mailPassword"));
                

                MailMessage message = PrepareMessage(contactViewModel, config);

                await smtpClient.SendMailAsync(message);
            }

            catch(SmtpException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private MailMessage PrepareMessage(ContactViewModel contactViewModel, NameValueCollection config)
        {
            string messageBody = string.Format("Name: {0}\nEmail: {1}\nMessage: \n{2}", 
                                 contactViewModel.ContactName, contactViewModel.ContactEmail, contactViewModel.ContactMessage);
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