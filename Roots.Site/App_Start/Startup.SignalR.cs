using Microsoft.Owin;
using Microsoft.Practices.ServiceLocation;
using Owin;
using Roots.BusinessLogic;
using Roots.Site.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roots.Site
{
    public partial class Startup
    {
        public static void ConfigureSignalR(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();            
            var domain = ServiceLocator.Current.GetInstance<DomainSupervisor>();
            NotificationsHub.WireEvents(domain);
        }

    }
}