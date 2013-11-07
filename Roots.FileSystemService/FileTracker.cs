using Roots.Connectors.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Roots.FileSystemService
{
    class FileTracker
    {

        private IDictionary<string, Track> tracks;
        private ICollection<Track> deletedFiles;
        private Action<Track> notificationCreatedCallback;
        private Action<Track> notificationChangedCallback;
        private Action<Track> notificationRenamedCallback;
        private Action<Track> notificationDeletedCallback;
        private object synclock = new object();

        public FileTracker(
            Action<Track> notificationCreatedCallback = null,
            Action<Track> notificationChangedCallback = null,
            Action<Track> notificationRenamedCallback = null,
            Action<Track> notificationDeletedCallback = null)
        {
            this.notificationCreatedCallback = notificationCreatedCallback;
            this.notificationChangedCallback = notificationChangedCallback;
            this.notificationRenamedCallback = notificationRenamedCallback;
            this.notificationDeletedCallback = notificationDeletedCallback;

            tracks = new Dictionary<string, Track>();
            deletedFiles = new List<Track>();
        }

        public FileTracker(FileTracker other)
        {
            lock (other.synclock)
            {
                this.notificationCreatedCallback = other.notificationCreatedCallback;
                this.notificationChangedCallback = other.notificationChangedCallback;
                this.notificationRenamedCallback = other.notificationRenamedCallback;
                this.notificationDeletedCallback = other.notificationDeletedCallback;

                tracks = other.tracks;
                deletedFiles = other.deletedFiles;
                foreach (var track in tracks.Values)
                {
                    track.tracker = this;
                }
                foreach (var track in deletedFiles)
                {
                    track.tracker = this;
                }
            }
        }

        public ITrack Track(string fullPath)
        {
            Track track;
            if (!tracks.TryGetValue(fullPath, out track))
            {
                track = new Track(this, fullPath);
                tracks[fullPath] = track;
            }
            return track;
        }

        internal void ChangeName(string oldFullPath, string newFullPath)
        {
            Track track;
            if (!tracks.TryGetValue(oldFullPath, out track)) throw new ArgumentOutOfRangeException("oldFullPath", oldFullPath, "not found");

            Track targetTrack;
            if (tracks.TryGetValue(newFullPath, out targetTrack) && (targetTrack.IsValid) && (targetTrack != track)) throw new InvalidOperationException("newFullPath already exists");

            tracks.Remove(oldFullPath);
            tracks[newFullPath] = track;
        }

        internal void MarkDeleted(Track track)
        {
            if (deletedFiles.Contains(track)) return;
            deletedFiles.Add(track);
            tracks.Remove(track.FullPath);
        }

       

        public IEnumerable<string> GetModifiedFilesPath()
        {
            var modifiedFiles  = Enumerable.Union(
                tracks.Select(x => x.Value).Where(x => x.IsValid),
                deletedFiles.Where(x => x.IsValid))
                .Select(x=>x.FullPath);
            return modifiedFiles;            

        }

        internal void Notify(Action<Track> action,Track track)
        {
            if (action != null)
            {
                Task.Run(() => action(track));
            }
        }

        internal void NotifyRenamed(Track track)
        {
            Notify(notificationRenamedCallback, track);
        }

        internal void NotifyDeleted(Track track)
        {
            Notify(notificationDeletedCallback, track);
        }

        internal void NotifyCreated(Track track)
        {
            Notify(notificationCreatedCallback, track);
        }

        internal void NotifyChanged(Track track)
        {
            Notify(notificationChangedCallback, track);
        }
    }
}
