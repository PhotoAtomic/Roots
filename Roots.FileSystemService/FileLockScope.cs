using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.FileSystemService
{
    public class FileLockScope : IDisposable
    {
        FileStream file;

        public FileLockScope(FileStream file)
        {
            this.file = file;
            file.Lock(0, file.Length);
        }

        public void Dispose()
        {
            file.Unlock(0, file.Length);
        }
    }
}
