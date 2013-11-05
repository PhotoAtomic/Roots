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
        readonly Regex folderFilter = new Regex("directory",RegexOptions.CultureInvariant|RegexOptions.IgnoreCase);

        public FileSystemWatcherService()
        {

            var folders = ConfigurationManager.AppSettings.GetValues("directory");

            tracker = new FileTracker(SendUpdate);
            eventWatcher = new FileSystemEventWatcher(tracker,folders.First());            
        }

        private void SendUpdate(FileTracker tracker)
        {
            var modifiedFiles = tracker.GetModifiedFilesPath();

            foreach (var fileName in modifiedFiles)
            {

                using (var file = File.OpenRead(fileName))
                {

                    byte[] data = new byte[file.Length];
                    using (file.LockScope())
                    {

                        file.ReadAsync(data, 0, (int)file.Length).Wait();
                    }
                    var destination = fileName
                        .Replace(ConfigurationManager.AppSettings.Get("Directory"), string.Empty)
                        .TrimStart('/', '\\');

                    string mimeType = "application/octet-stream";
                    var extension = Path.GetExtension(fileName);
                    if (extension == ".pdf") mimeType = "application/pdf";
                    else if (extension == ".txt") mimeType = "text/plain";
                    else if (extension == ".sd" || extension == ".mdl" || extension == ".sdf") mimeType = "chemical/x-mdl-sdf";

                    var contentDto = new FileContent
                    {
                        Source = fileName,
                        Destination = destination,
                        MimeType = mimeType,
                        Content = data,
                    };

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(contentDto);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");                    
                    HttpClient client = new HttpClient();

                    var baseUri = new Uri(ConfigurationManager.AppSettings.Get("ServerUri"));
                    //var request = new Uri(baseUri, "Content");
                    client.BaseAddress = baseUri;
                    HttpResponseMessage response = client.PostAsync("Content", content).Result;                    
                    
                }
            }

        }
     

        public void Start()
        {
			
        }

        public void Stop()
        {
			
        }

        
    }
}
