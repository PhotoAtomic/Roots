using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.FileSystemService
{
    public interface ITrack
    {
        void Renamed(string newFullPath);

        void Deleted();
          
        void Created();

        void Changed();
    }
}
