using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.FileSystemService
{
    public interface ITrack
    {
        Task Renamed(string newFullPath);

        Task Deleted();

        Task Created();

        Task Changed();
    }
}
