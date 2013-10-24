using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Baki;
using System.ComponentModel;

namespace Roots.FileSystemService
{
    /// <summary>
    /// WARNING: all this project is a test, it is not intented to be production grade code!
    /// </summary>
    static class Program
    {        
        static void Main()
        {            
            WindowsService.Run<FileSystemWatcherService>(null);            
        }
    }
}
