using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;
using System.ComponentModel.Composition;
using Roots.BusinessLogic.Selectors;

namespace Roots.Site.Hubs
{
    public class NotificationsHub : Hub
    {
        static Lazy<IHubContext> context = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>());

        public static void WireEvents(DomainSupervisor domain){
            domain.Apply(new MutationListener<NewFileUploaded>(NotifyNewFileUploaded));
            domain.Apply(new MutationListener<ExistingFileContentUpdated>(NotifyExistingFileUpdated));
            domain.Apply(new MutationListener<ExistingFileRenamed>(NotifyExistingFileRenamed));
            domain.Apply(new MutationListener<FileRemoved>(NotifyExistingFileRemoved));
        }

        private static void NotifyExistingFileRemoved(FileRemoved fileRemoved)
        {
            context.Value.Clients.All.itemRemoved(fileRemoved.SourceName);
        }

        private static void NotifyExistingFileRenamed(ExistingFileRenamed fileRenamed)
        {
            context.Value.Clients.All.itemRenamed(fileRenamed.OldSourceName, fileRenamed.NewSourceName);
        }

        private static void NotifyExistingFileUpdated(ExistingFileContentUpdated fileUploaded)
        {
            context.Value.Clients.All.itemUpdated(fileUploaded.SourceName);            
        }

        private static void NotifyNewFileUploaded(NewFileUploaded newFileUploaded)
        {
            context.Value.Clients.All.itemAdded(newFileUploaded.SourceName);
        }

        private DomainSupervisor domain;

        [ImportingConstructor]
        public NotificationsHub([Import]DomainSupervisor domain)
        {
            this.domain = domain;
        }

        public override System.Threading.Tasks.Task OnConnected()
        {            
            Clients.Caller.allItems(domain.Apply(new SelectAllFiles()).ToArray());
            return base.OnConnected();         
        }
       
    }
}