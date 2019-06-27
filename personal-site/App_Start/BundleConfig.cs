using System.Web;
using System.Web.Optimization;

namespace personal_site
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/main.css"));
        }
    }
}
