using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace personal_site.Filters
{
    public class SiteModeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            bool shutdownMode = appSettings.Get("ShutdownMode").Equals("true");
            bool maintMode = appSettings.Get("MaintenanceMode").Equals("true");

            if (shutdownMode || maintMode)
                filterContext.Result = new RedirectResult("~/Error/MaintenanceMode");

            else base.OnActionExecuting(filterContext);
        }
    }
}