using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Roots.FileSystemService
{
    public class FileLockScope : IDisposable
    {
        FileStream file;
        int retries = 500;
        public FileLockScope(FileStream file)
        {
            this.file = file;
            bool lockAcquired = false;
            int waitTime = 1;
            do
            {
                if (retries <= 0) throw new TimeoutException("Maximum number of retries reached, unable to acquire lock on file");
                try
                {
                    file.Lock(0, file.Length);
                    lockAcquired = true;
                }
                catch (Exception)
                {
                    lockAcquired = false;
                    Thread.Sleep(waitTime);
                    waitTime = Math.Min(1000 * 60 * 5, waitTime * 2);
                    retries--;
                }
            } while (!lockAcquired);
        }

        public void Dispose()
        {
            file.Unlock(0, file.Length);
        }
    }
}
