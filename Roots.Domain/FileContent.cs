using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Domain
{
   public class FileContent
    {
        public bool Approved { get; set; }

        public Guid Id { get; set; }

        public string Source { get; set; }
        public string SourceName { get; set; }

        public byte[] Data { get; set; }
        public string MimeType { get; set; }       

        public DateTime CreateDate { get; set; }
        public DateTime LastModify { get; set; }
    }
}
