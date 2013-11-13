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
using Roots.SupportedFileTypes;

namespace Roots.FileSystemService
{
    class FileSystemWatcherService : IWindowsServiceHost
    {

        FileTracker tracker;
        FileSystemEventWatcher eventWatcher;
        readonly Regex folderFilter = new Regex("directory", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        readonly WebApiClient client;
        readonly string source;

        public FileSystemWatcherService()
        {

            var folders = ConfigurationManager.AppSettings.GetValues("Directory");

            source = ConfigurationManager.AppSettings["SourceIdentifier"];

            client = new WebApiClient(new Uri(ConfigurationManager.AppSettings.Get("ServerUri")));

            tracker = new FileTracker(
                notificationCreatedCallback: SendCreate,
                notificationChangedCallback: SendUpdate,
                notificationRenamedCallback: SendRename,
                notificationDeletedCallback: SendDelete);

            new Synchronizer(source, client).Synchronize(tracker, folders.First());
            eventWatcher = new FileSystemEventWatcher(tracker, folders.First());
            
        }

        private void SendUpdate(Track track)
        {
            var fileName = track.FullPath;

            var contentDto = PrepareContentDto(fileName);

            var machinePath = FileTracker.MakeMachinePath(fileName);

            var result = client.PutAsync(
                "Content", 
                new 
                {
                    Source = source, 
                    Id = machinePath 
                }, 
                contentDto).Result;
        
        }


        private void SendDelete(Track track)
        {
            var name = FileTracker.MakeMachinePath(track.FullPath);

            var result = client.DeleteAsync(
                    "Content", 
                    new { 
                        Source = source,
                        Id = name 
                    }).Result;
        }

        private void SendRename(Track track)
        {
            var oldName = FileTracker.MakeMachinePath(track.OriginalFullPath);
            var newName = FileTracker.MakeMachinePath(track.FullPath);            

            var result = client.PutAsync(
                "Content", 
                new 
                { 
                    Source = source,
                    Id = oldName, 
                    NewName = newName 
                }).Result;
        }

        private void SendCreate(Track track)
        {
            var fileName = track.FullPath;
            var machinePath = FileTracker.MakeMachinePath(fileName);
            var contentDto = PrepareContentDto(fileName);

            var result = client.PostAsync(
                "Content",
                new
                {
                    Source = source,
                    Id = machinePath,
                },
                contentDto
                ).Result;

        }


        private static FileContent PrepareContentDto(string fileName)
        {
            byte[] data = ReadData(fileName);            

            string mimeType = GetMimeType(fileName);

            var contentDto = new FileContent
            {
                MimeType = mimeType,
                Content = data,
            };
            return contentDto;
        }

        private static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            string mimeType = MimeTypes.GetFileType(extension) ?? MimeTypes.OctetStream;
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
                file.Close();
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
