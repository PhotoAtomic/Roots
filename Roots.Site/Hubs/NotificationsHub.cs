using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Roots.Site.Hubs
{
    public class NotificationsHub : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            
            Clients.Caller.allItems(new string[] { "ciao", "cane" });
            return base.OnConnected();         
        }
       
    }
}