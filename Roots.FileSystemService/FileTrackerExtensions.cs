using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.FileSystemService
{
    public static class FileTrackerExtensions
    {
        public static void Renamed(this IEnumerable<Track> tracks, string oldPath, string newPath)
        {
            foreach (var track in new List<Track>(tracks))
            {
                var newFullPath = track.FullPath.Replace(oldPath, newPath);
                track.Renamed(newFullPath);
            }
        }
        public static void Deleted(this IEnumerable<Track> tracks)
        {
            foreach (var track in new List<Track>(tracks))
            {
                track.Deleted();
            }
        }        
    }
}
