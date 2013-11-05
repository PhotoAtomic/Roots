using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Roots.Site.Startup))]
namespace Roots.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureSignalR(app);
        }
    }
}
