using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Domain
{
   public class FileContent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string MimeType { get; set; }      
    }
}
