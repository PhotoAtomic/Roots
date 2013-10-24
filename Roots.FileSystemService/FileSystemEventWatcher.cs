using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.FileSystemService
{
    class FileSystemEventWatcher
    {

        private FileSystemWatcher watcher;
        private FileTracker tracker;

        public FileSystemEventWatcher(FileTracker tracker,string rootPath)
        {
            this.tracker = tracker;
                

            watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = true;
            watcher.Path = rootPath;
            watcher.NotifyFilter = 
                NotifyFilters.LastWrite | 
                NotifyFilters.Size | 
                NotifyFilters.DirectoryName | 
                NotifyFilters.CreationTime | 
                NotifyFilters.FileName;
            watcher.InternalBufferSize = 65536;
            
            watcher.Changed += watcher_Changed;
            watcher.Created += watcher_Created;
            watcher.Deleted += watcher_Deleted;
            watcher.Renamed += watcher_Renamed;

            watcher.EnableRaisingEvents = true;
        }
     

        void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            tracker.Track(e.OldFullPath).Renamed(e.FullPath);           
        }

        void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted) return;
            tracker.Track(e.FullPath).Deleted();
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Created) return;
            tracker.Track(e.FullPath).Created();
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    tracker.Track(e.FullPath).Changed();
                    break;
                case WatcherChangeTypes.Created:
                    tracker.Track(e.FullPath).Created();
                    break;
                case WatcherChangeTypes.Deleted:
                    tracker.Track(e.FullPath).Deleted();
                    break;
                default:
                    break;
            }            
        }
    }
}
