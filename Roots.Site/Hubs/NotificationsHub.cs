using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;

namespace Roots.Site.Hubs
{
    public class NotificationsHub : Hub
    {        
        public static void WireEvents(DomainSupervisor domain){
            domain.Apply(new MutationListener<NewFileUploaded>(NotifyNewFileUploaded));
        }

        private static void NotifyNewFileUploaded(NewFileUploaded newFileUploaded)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            context.Clients.All.itemAdded(newFileUploaded.Name);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {            
            Clients.Caller.allItems(new string[] { "ciao", "cane" });
            return base.OnConnected();         
        }
       
    }
}