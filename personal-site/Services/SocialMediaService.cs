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

                SetSocialNodeValue("Github", model.GithubLink, socialNode);
                SetSocialNodeValue("Twitter", model.GithubLink, socialNode);
                SetSocialNodeValue("Dribble", model.GithubLink, socialNode);
                SetSocialNodeValue("Rss", model.GithubLink, socialNode);
                SetSocialNodeValue("StackOverflow", model.GithubLink, socialNode);

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
                GithubLink = socialConfig.Get("Github"),
                TwitterLink = socialConfig.Get("Twitter"),
                DribbleLink = socialConfig.Get("Dribble"),
                RssLink = socialConfig.Get("Rss"),
                StackOverflowLink = socialConfig.Get("StackOverflow")
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