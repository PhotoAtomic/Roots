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

        public FileLockScope(FileStream file)
        {
            this.file = file;
            bool lockAcquired = false;
            int waitTime = 1;
            do
            {
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
                }
            } while (!lockAcquired);
        }

        public void Dispose()
        {
            file.Unlock(0, file.Length);
        }
    }
}
