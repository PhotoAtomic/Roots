using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Extractors
{
    public class FilePreview
    {
        public Guid Id { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        //public byte[] Content { get; set; }
    }
}
