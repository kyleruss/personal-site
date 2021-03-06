﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string ProfilePicture { get; set; }

        public string Provider { get; set; }

        private DateTime? dateJoined = null;

        public DateTime DateJoined
        {
            get { return this.dateJoined ?? DateTime.Now; }
            set { this.dateJoined = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}