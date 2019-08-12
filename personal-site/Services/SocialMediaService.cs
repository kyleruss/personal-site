using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;
using personal_site.ViewModels;

namespace personal_site.Services
{
    public class SocialMediaService
    {
        private static SocialMediaService _instance;
        public AdminSocialMediaViewModel SocialModel { get; set; }

        private SocialMediaService() { }

        public bool UpdateSocialConfig(AdminSocialMediaViewModel model) 
        {
            try
            {
                var confDoc = new XmlDocument();
                confDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                SetSocialNodeValue("Github", model.Github, confDoc);
                SetSocialNodeValue("Twitter", model.Twitter, confDoc);
                SetSocialNodeValue("Dribble", model.Dribble, confDoc);
                SetSocialNodeValue("Rss", model.Rss, confDoc);
                SetSocialNodeValue("StackOverflow", model.StackOverflow, confDoc);

                confDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                ConfigurationManager.RefreshSection("socialSettings");
                SocialModel = model;
                return true;
            }

            catch(Exception e)
            {
                Debug.WriteLine("[Update social config error] " + e.Message);
                return false;
            }
        }

        public void InitSocialModel()
        {
            var socialConfig = ConfigurationManager.GetSection("socialSettings") as NameValueCollection;

            SocialModel = new AdminSocialMediaViewModel()
            {
                Github = socialConfig.Get("Github"),
                Twitter = socialConfig.Get("Twitter"),
                Dribble = socialConfig.Get("Dribble"),
                Rss = socialConfig.Get("Rss"),
                StackOverflow = socialConfig.Get("StackOverflow")
            };
        }

        private void SetSocialNodeValue(string id, string value, XmlDocument doc)
        {
            doc.SelectSingleNode("/configuration/socialSettings/add[@key='" + id + "']")
                .Attributes["value"].Value = value;
        }

        public static SocialMediaService GetInstance()
        {
            _instance = _instance ?? new SocialMediaService();
            return _instance;
        }
    }
}