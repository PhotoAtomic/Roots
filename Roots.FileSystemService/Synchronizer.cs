using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roots.FileSystemService
{
    public class Synchronizer
    {
        private WebApiClient client;

        public Synchronizer(WebApiClient client)
        {
            this.client = client;
        }
        public void Synchronize(FileTracker tracker, string rootPath)
        {
            var fileListFromDb = client.GetAsync<string[]>("Content", new { path = FileTracker.MakeMachinePath(rootPath)}).Result;
            var fileListFromFs = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories);
            var filesToRemove = fileListFromDb.Except(fileListFromFs);
            var filesToAdd = fileListFromFs.Except(fileListFromDb);


            foreach (var fileName in filesToRemove)
            {
                tracker.Track(fileName).Deleted().Wait();
            }
            foreach (var fileName in filesToAdd)
            {
                tracker.Track(fileName).Created().Wait();
            }
        }
    }
}
