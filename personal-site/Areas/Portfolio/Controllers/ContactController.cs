using personal_site.Filters;
using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace personal_site.Areas.Portfolio.Controllers
{
    [SiteModeFilter]
    public class ContactController : Controller
    {

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMessage(ContactViewModel contactViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ControllerHelper.GetModelStateErrors(ModelState);
                return ControllerHelper.JsonActionResponse(false, "Invalid input", errorList);
            }

            MailService mailService = MailService.GetInstance();
            MailMessage message = mailService.PrepareContactMessage(contactViewModel);
            bool msgSent        =   await mailService.SendMessage(message);
            string responseMsg  =   msgSent? "Message sent!" : "Failed to send message";

            return ControllerHelper.JsonActionResponse(msgSent, responseMsg);
        }
    }
}