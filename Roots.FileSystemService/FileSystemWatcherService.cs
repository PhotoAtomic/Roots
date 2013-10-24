using System;
using Baki;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

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
            
        }
     

        public void Start()
        {
			
        }

        public void Stop()
        {
			
        }

        
    }
}
