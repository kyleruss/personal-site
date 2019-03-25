using personal_site.Services;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Controllers
{
    public class ContactController : Controller
    {

        [HttpPost]
        public ActionResult SendMessage(ContactViewModel contactViewModel)
        {
            ContactService contactService = ContactService.GetInstance();
            contactService.SendMessage(contactViewModel);

            return Content("Message Sent");
        }
    }
}