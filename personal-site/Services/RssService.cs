using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;

namespace personal_site.Services
{
    public class RssService
    {
        private static RssService _instance;

        private RssService() { }

        public bool UpdateChannel(AdminRssChannelViewModel model)
        {
            try
            {
                XmlDocument rssDoc = GetRssXmlDoc();
                XmlNode channelNode = rssDoc.SelectSingleNode("/rss/channel");
                channelNode.SelectSingleNode("/title").InnerText = model.Name;
                channelNode.SelectSingleNode("/description").InnerText = model.Description;
                channelNode.SelectSingleNode("/image").InnerText = model.ImageUrl;

                SaveRssXmlDoc(rssDoc);
                return true;
            }

            catch(Exception e)
            {
                Debug.WriteLine("[Rss-UpdateChannel Exception] " + e.Message);
                return false;
            }
        }

        public bool PushUpdate(AdminRssItemViewModel model)
        {
            try
            {
                XmlDocument rssDoc = GetRssXmlDoc();
                XmlNode itemsNode = rssDoc.SelectSingleNode("/rss/items");
                XmlNode currentItemNode = rssDoc.CreateElement("item");

                itemsNode.PrependChild(currentItemNode);
                SaveRssXmlDoc(rssDoc);
                return true;
            }

            catch (Exception e)
            {
                Debug.WriteLine("[Rss-PushUpdate Exception] " + e.Message);
                return false;
            }
        }

        public bool RemoveItem(int id)
        {
            try
            {
                XmlDocument rssDoc = GetRssXmlDoc();
                XmlNode itemsNode = rssDoc.SelectSingleNode("/rss/items/item[@id='" + id + "'");
                itemsNode.ParentNode.RemoveChild(itemsNode);

                SaveRssXmlDoc(rssDoc);
                return true;
            }

            catch (Exception e)
            {
                Debug.WriteLine("[Rss-RemoveItem Exception] " + e.Message);
                return false;
            }
        }

        private XmlDocument GetRssXmlDoc()
        {
            string path = GetRssXmlPath();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            return xmlDoc;
        }

        private void SaveRssXmlDoc(XmlDocument doc)
        {
            string path = GetRssXmlPath();
            doc.Save(path);
        }

        private string GetRssXmlPath()
        {
            return HttpContext.Current.Server.MapPath("~/Content/resources/blog-rss.xml");
        }

        public static RssService GetInstance()
        {
            _instance = _instance ?? new RssService();
            return _instance;
        }
    }
}