using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(personal_site.Startup))]
namespace personal_site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
