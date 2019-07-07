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
                UpdateChannelItem("title", model.Name, rssDoc);
                UpdateChannelItem("description", model.Description, rssDoc);
                UpdateChannelItem("image", model.ImageUrl, rssDoc);

                SaveRssXmlDoc(rssDoc);
                return true;
            }

            catch(Exception e)
            {
                Debug.WriteLine("[Rss-UpdateChannel Exception] " + e.Message);
                return false;
            }
        }

        public AdminRssChannelViewModel GetChannelSettings()
        {
            XmlDocument rssDoc = GetRssXmlDoc();
            AdminRssChannelViewModel model = new AdminRssChannelViewModel()
            {
                Name = GetChannelItem("title", rssDoc).InnerText,
                Description = GetChannelItem("description", rssDoc).InnerText,
                ImageUrl = GetChannelItem("image", rssDoc).InnerText
            };

            return model;
        }
        

        public bool PushUpdate(AdminRssItemViewModel model)
        {
            try
            {
                XmlDocument rssDoc = GetRssXmlDoc();
                XmlNode itemsNode = rssDoc.SelectSingleNode("/rss/channel");
                XmlNode currentItemNode = rssDoc.CreateElement("item");
                XmlNode itemTitleNode = rssDoc.CreateElement("title");
                XmlNode itemLinkNode = rssDoc.CreateElement("link");
                XmlNode itemDescNode = rssDoc.CreateElement("description");

                string itemTitle = model.ItemTitle ?? "New update";
                string itemLink = model.ItemLink ?? "www.kylerussell.co.nz";
                itemTitleNode.InnerText = itemTitle;
                itemLinkNode.InnerText = itemLink;
                itemDescNode.InnerText = model.ItemContent;

                currentItemNode.AppendChild(itemTitleNode);
                currentItemNode.AppendChild(itemLinkNode);
                currentItemNode.AppendChild(itemDescNode);
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

        public List<AdminRssItemViewModel> GetRssItems()
        {
            XmlDocument rssDoc = GetRssXmlDoc();
            XmlNodeList rssItemNodeList = rssDoc.SelectNodes("/rss/channel/item");
            List<AdminRssItemViewModel> itemList = new List<AdminRssItemViewModel>();
             
            foreach(XmlNode itemNode in rssItemNodeList)
            {
                AdminRssItemViewModel currentItemModel = new AdminRssItemViewModel()
                {
                    ItemTitle = itemNode.SelectSingleNode("./title").InnerText,
                    ItemContent = itemNode.SelectSingleNode("./description").InnerText,
                    ItemLink = itemNode.SelectSingleNode("./link").InnerText
                };

                itemList.Add(currentItemModel);
            }

            return itemList;
        }

        public bool RemoveItem(int index)
        {
            try
            {
                XmlDocument rssDoc = GetRssXmlDoc();
                XmlNodeList itemsList = rssDoc.SelectNodes("/rss/channel/item");
                XmlNode itemNode = itemsList[index];
                itemNode.ParentNode.RemoveChild(itemNode);

                SaveRssXmlDoc(rssDoc);
                return true;
            }

            catch (Exception e)
            {
                Debug.WriteLine("[Rss-RemoveItem Exception] " + e.Message);
                return false;
            }
        }

        private void UpdateChannelItem(string name, string value, XmlDocument doc)
        {
            GetChannelItem(name, doc).InnerText = value;
        }

        private XmlNode GetChannelItem(string name, XmlDocument doc)
        {
            return doc.SelectSingleNode("/rss/channel/" + name);
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