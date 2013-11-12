using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;
using System.ComponentModel.Composition;
using Roots.BusinessLogic.Extractors;

namespace Roots.Site.Hubs
{
    public class NotificationsHub : Hub
    {
        static Lazy<IHubContext> context = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>());

        public static void WireEvents(DomainSupervisor domain){
            domain.Apply(new MutationListener<NewFileUploader>(NotifyNewFileUploaded));
            domain.Apply(new MutationListener<ExistingFileContentUpdated>(NotifyExistingFileUpdated));
            domain.Apply(new MutationListener<ExistingFileRenamer>(NotifyExistingFileRenamed));
            domain.Apply(new MutationListener<FileRemover>(NotifyExistingFileRemoved));
        }

        private static void NotifyExistingFileRemoved(FileRemover fileRemoved)
        {
            context.Value.Clients.All.itemRemoved(fileRemoved.IdOfTheRemovedFiles);
        }

        private static void NotifyExistingFileRenamed(ExistingFileRenamer fileRenamed)
        {
            context.Value.Clients.All.itemRenamed(fileRenamed.IdOfTheRenamedFile, fileRenamed.NewSourceName);
        }

        private static void NotifyExistingFileUpdated(ExistingFileContentUpdated fileUploaded)
        {
            context.Value.Clients.All.itemUpdated(fileUploaded.IdOfUpdatedFile);            
        }

        private static void NotifyNewFileUploaded(NewFileUploader newFileUploaded)
        {
            var filePreview = new FilePreview
            {
                Id = newFileUploaded.IdOfTheNewFile,
                Name = newFileUploaded.SourceName,
                MimeType = newFileUploaded.MimeType,
            };
            if (newFileUploaded.MimeType == "chemical/x-mdl-sdf") filePreview.Content = newFileUploaded.FileContent;
            context.Value.Clients.All.itemAdded(filePreview);
        }

        private DomainSupervisor domain;

        [ImportingConstructor]
        public NotificationsHub([Import]DomainSupervisor domain)
        {
            this.domain = domain;
        }

        public override System.Threading.Tasks.Task OnConnected()
        {            
            Clients.Caller.allItems(domain.Apply(new SelectAllApprovedFiles()).ToArray());
            return base.OnConnected();         
        }
       
    }
}