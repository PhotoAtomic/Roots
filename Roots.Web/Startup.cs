using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Roots.Web.Startup))]
namespace Roots.Web
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
