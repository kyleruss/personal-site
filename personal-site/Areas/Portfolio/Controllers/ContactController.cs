using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace personal_site.Areas.Portfolio.Controllers
{
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

            ContactService contactService = ContactService.GetInstance();
            bool msgSent        =   await contactService.SendMessage(contactViewModel);
            string responseMsg  =   msgSent? "Message sent!" : "Failed to send message";

            return ControllerHelper.JsonActionResponse(msgSent, responseMsg);
        }
    }
}