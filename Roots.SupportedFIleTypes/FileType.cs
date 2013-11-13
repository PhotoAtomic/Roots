using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.SupportedFileTypes
{
    public class FileType
    {

        internal FileType(string mimeType, params string[] extensions)
        {
            if(mimeType == null)throw new ArgumentNullException(mimeType);
            MimeType = mimeType;
            this.Extensions = extensions;
            
        }

        public string MimeType { get; private set; }
        public IEnumerable<string> Extensions { get; private set; }

        public bool MatchExtension(string extension)
        {
            if (Extensions == null) return false;
            return Extensions.Any(x => string.Equals(x, extension, StringComparison.InvariantCultureIgnoreCase));
        }

        public static implicit operator String(FileType fileType)
        {
            return fileType.MimeType;
        }

        public static bool operator == (string a, FileType b)
        {
            if (a == null && Object.ReferenceEquals(b, null)) return true;
            if (a == null ^ Object.ReferenceEquals(b, null)) return false;
            return string.Equals(a,b.MimeType,StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator !=(string a, FileType b)
        {
            if (a == null && Object.ReferenceEquals(b, null)) return true;
            if (a == null ^ Object.ReferenceEquals(b, null)) return false;
            return !string.Equals(a, b.MimeType, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator == (FileType a, string b)
        {
            if (Object.ReferenceEquals(a, null) && b == null) return true;
            if (Object.ReferenceEquals(a, null) ^ b == null) return false;
            return string.Equals(b,a.MimeType,StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator !=(FileType a, string b)
        {
            if (Object.ReferenceEquals(a, null) && b == null) return true;
            if (Object.ReferenceEquals(a, null) ^ b == null) return false;
            return !string.Equals(b, a.MimeType, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is string)
            {
                return this == (string)obj;
            }
            if (obj is FileType)
            {
                return string.Equals(this.MimeType, ((FileType)obj).MimeType, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        public override int GetHashCode()
        {
            if (MimeType == null) return 0;
            return MimeType.GetHashCode();
        }
    }
}
