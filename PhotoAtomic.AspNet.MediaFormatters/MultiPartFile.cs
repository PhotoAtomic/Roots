using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.AspNet.MediaFormatters
{
    public class MultiPartFile
    {
        public string MimeType { get; private set; }
        public Stream Stream { get; private set; }
        public string Name { get; private set; }

        public MultiPartFile(string name, string mimeType, Stream stream)
        {
            Stream = stream;
            MimeType = mimeType;
            Name = name;
        }

        async public Task<byte[]> AsByteArray()
        {
            var buffer = new byte[Stream.Length];
            await Stream.ReadAsync(buffer, 0, (int)Stream.Length);            
            return buffer;
        }

    }
}
