using System;
using Baki;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using Roots.Connectors.Interfaces;
using System.Text;

namespace Roots.FileSystemService
{
    class FileSystemWatcherService : IWindowsServiceHost
    {

        FileTracker tracker;
        FileSystemEventWatcher eventWatcher;
        readonly Regex folderFilter = new Regex("directory", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        readonly WebApiClient client;

        public FileSystemWatcherService()
        {

            var folders = ConfigurationManager.AppSettings.GetValues("directory");

            client = new WebApiClient(new Uri(ConfigurationManager.AppSettings.Get("ServerUri")));

            tracker = new FileTracker(
                notificationCreatedCallback: SendCreate,
                notificationChangedCallback: SendUpdate,
                notificationRenamedCallback: SendRename,
                notificationDeletedCallback: SendDelete);

            new Synchronizer(client).Synchronize(tracker, folders.First());
            eventWatcher = new FileSystemEventWatcher(tracker, folders.First());
            
        }

        private void SendUpdate(Track track)
        {
            var fileName = track.FullPath;

            var contentDto = PrepareContentDto(fileName);

            var machinePath = FileTracker.MakeMachinePath(fileName);

            var result = client.PutAsync("Content", new { Id = machinePath }, contentDto).Result;
        
        }


        private void SendDelete(Track track)
        {
            var name = FileTracker.MakeMachinePath(track.FullPath);

            var result = client.DeleteAsync("Content", new { Id = name }).Result;
        }

        private void SendRename(Track track)
        {
            var oldName = FileTracker.MakeMachinePath(track.OriginalFullPath);
            var newName = FileTracker.MakeMachinePath(track.FullPath);            

            var result = client.PutAsync("Content", new { Id = oldName, NewName = newName }).Result;
        }

        private void SendCreate(Track track)
        {
            var fileName = track.FullPath;

            var contentDto = PrepareContentDto(fileName);

            var result = client.PostAsync("Content", contentDto).Result;

        }


        private static FileContent PrepareContentDto(string fileName)
        {
            byte[] data = ReadData(fileName);

            var destination = FileTracker.MakeMachinePath(fileName);

            string mimeType = GetMimeType(fileName);

            var contentDto = new FileContent
            {
                Source = fileName,
                Destination = destination,
                MimeType = mimeType,
                Content = data,
            };
            return contentDto;
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/octet-stream";
            var extension = Path.GetExtension(fileName);
            if (extension == ".pdf") mimeType = "application/pdf";
            else if (extension == ".txt") mimeType = "text/plain";
            else if (extension == ".jpg" || extension == ".jpeg") mimeType = "image/jpeg";
            else if (extension == ".sd" || extension == ".mdl" || extension == ".sdf") mimeType = "chemical/x-mdl-sdf";
            return mimeType;
        }

        private static byte[] ReadData(string fileName)
        {
            byte[] data;
            using (var file = File.OpenRead(fileName))
            {
                data = new byte[file.Length];
                using (file.LockScope())
                {
                    file.ReadAsync(data, 0, (int)file.Length).Wait();
                }
            }
            return data;
        }


        public void Start()
        {

        }

        public void Stop()
        {

        }
        
    }
}
