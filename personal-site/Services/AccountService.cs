using LinqToTwitter;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using personal_site.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services
{
    public class AccountService
    {
        private static AccountService _instance;

        private AccountService() { }

        public static AccountService GetInstance()
        {
            _instance = _instance ?? new AccountService();
            return _instance;
        }
    }
}