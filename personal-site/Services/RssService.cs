using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace personal_site.Services
{
    public class RssService
    {
        private static RssService _instance;
        

        private RssService() { }

        public void UpdateChannel(AdminRssChannelViewModel model)
        {
            XmlDocument rssDoc = GetRssXmlDoc();
            XmlNode channelNode = rssDoc.SelectSingleNode("/rss/channel");
            channelNode.SelectSingleNode("/title").InnerText = model.Name;
            channelNode.SelectSingleNode("/description").InnerText = model.Description;
            channelNode.SelectSingleNode("/image").InnerText = model.ImageUrl;

            SaveRssXmlDoc(rssDoc);
        }

        public void PushUpdate(AdminRssItemViewModel model)
        {
            XmlDocument rssDoc = GetRssXmlDoc();
            XmlNode itemsNode = rssDoc.SelectSingleNode("/rss/items");
            XmlNode currentItemNode = rssDoc.CreateElement("item");

            itemsNode.PrependChild(currentItemNode);
            SaveRssXmlDoc(rssDoc);
        }

        public void RemoveItem(int id)
        {
            XmlDocument rssDoc = GetRssXmlDoc();
            XmlNode itemsNode = rssDoc.SelectSingleNode("/rss/items/item[@id='" + id + "'");
            itemsNode.ParentNode.RemoveChild(itemsNode);

            SaveRssXmlDoc(rssDoc);
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