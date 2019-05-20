using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace personal_site.Services
{
    public class RssService
    {
        private static RssService _instance;

        private RssService() { }

        public void UpdateChannel(AdminRssChannelViewModel model)
        {

        }

        public void PushUpdate(AdminRssItemViewModel model)
        {

        }

        public void RemoveItem(int id)
        {

        }

        public static RssService GetInstance()
        {
            _instance = _instance ?? new RssService();
            return _instance;
        }
    }
}