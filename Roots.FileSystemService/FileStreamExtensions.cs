using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.FileSystemService
{
    public static class FileStreamExtensions
    {
        public static FileLockScope LockScope(this FileStream file)
        {
            return new FileLockScope(file);
        }
    }
}
