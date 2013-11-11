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
        private string source;

        public Synchronizer(string source, WebApiClient client)
        {
            this.source = source;
            this.client = client;
        }
        public void Synchronize(FileTracker tracker, string rootPath)
        {
            var fileListFromDb = client.GetAsync<string[]>("Content", new { Source = source, path = FileTracker.MakeMachinePath(rootPath)}).Result;
            var fileListFromFs = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories).Select(x=>FileTracker.MakeMachinePath(x));
            var filesToRemove = fileListFromDb.Except(fileListFromFs);
            var filesToAdd = fileListFromFs.Except(fileListFromDb);

            //throw new NotImplementedException("finire di implementare la sincronizzazione: deve tenere conto dei machine path e dei path locali");

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
