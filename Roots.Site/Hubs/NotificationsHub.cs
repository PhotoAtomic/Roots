using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;
using System.ComponentModel.Composition;
using Roots.BusinessLogic.Extractors;
using System.IO;
using Roots.Domain;
using Roots.SupportedFileTypes;

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
            context.Value.Clients.All.itemRenamed(fileRenamed.IdOfTheRenamedFile, Path.GetFileNameWithoutExtension(fileRenamed.NewSourceName));
        }

        private static void NotifyExistingFileUpdated(ExistingFileContentUpdated fileUploaded)
        {
            var filePreview = new FilePreview
            {
                Id = fileUploaded.IdOfUpdatedFile,
                Name = Path.GetFileNameWithoutExtension(fileUploaded.SourceName),
                MimeType = fileUploaded.MimeType,
            };
           // if (fileUploaded.MimeType == MimeTypes.Chemical) filePreview.Content = fileUploaded.FileContent;
            context.Value.Clients.All.itemUpdated(filePreview);            
        }

        private static void NotifyNewFileUploaded(NewFileUploader newFileUploaded)
        {
            var filePreview = new FilePreview
            {
                Id = newFileUploaded.IdOfTheNewFile,
                Name = Path.GetFileNameWithoutExtension(newFileUploaded.SourceName),
                MimeType = newFileUploaded.MimeType,
            };
            //if (newFileUploaded.MimeType == MimeTypes.Chemical) filePreview.Content = newFileUploaded.FileContent;
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
            var files = domain.Apply(new SelectAllApprovedFiles()).Select(x =>
                new FilePreview
                {
                    Id = x.Id,
                    Name = Path.GetFileNameWithoutExtension(x.SourceName),
                    MimeType = x.MimeType,
                    //Content = (x.MimeType == MimeTypes.Chemical)? x.Data : null,
                });
            Clients.Caller.allItems(files);
            return base.OnConnected();         
        }
       
    }
}