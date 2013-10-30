using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Connectors.Interfaces
{
    public class FileContent
    {        
        public string Source { get; set; }
        public string Destination { get; set; }
        public string MimeType { get; set; }                
        public byte[] Content { get; set; }
    }
}
