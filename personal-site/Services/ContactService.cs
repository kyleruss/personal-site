using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace personal_site.Services
{
    public class ContactService
    {
        private static ContactService _instance;

        private ContactService() { }

        public void SendMessage()
        {

        }

        public static ContactService GetInstance()
        {
            _instance = _instance ?? new ContactService();
            return _instance;
        }
    }
}