using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using personal_site.ViewModels;

namespace personal_site.Services
{
    public class SocialMediaService
    {
        private static SocialMediaService _instance;
        public AdminSocialMediaViewModel socialModel { get; set; }

        private SocialMediaService() { }

        public static SocialMediaService GetInstance()
        {
            _instance = _instance ?? new SocialMediaService();
            return _instance;
        }
    }
}