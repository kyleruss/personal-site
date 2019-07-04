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
                XmlNode socialNode = confDoc.SelectSingleNode("//socialSettings");

                SetSocialNodeValue("Github", model.Github, socialNode);
                SetSocialNodeValue("Twitter", model.Twitter, socialNode);
                SetSocialNodeValue("Dribble", model.Dribble, socialNode);
                SetSocialNodeValue("Rss", model.Rss, socialNode);
                SetSocialNodeValue("StackOverflow", model.StackOverflow, socialNode);

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

        public void initSocialModel()
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

        private void SetSocialNodeValue(string id, string value, XmlNode socialNode)
        {
            socialNode.SelectSingleNode("/add[@key='" + id + "']")
                .Attributes["value"].Value = value;
        }

        public static SocialMediaService GetInstance()
        {
            _instance = _instance ?? new SocialMediaService();
            return _instance;
        }
    }
}