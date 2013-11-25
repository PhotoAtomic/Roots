using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.AspNet.MediaFormatters
{
    public static class HttpContentExtensions
    {
        public static bool IsJson(this HttpContent content)
        {             
             return  
                content != null &&
                content.Headers != null &&
                content.Headers.ContentType != null &&
                string.Equals(content.Headers.ContentType.MediaType, "application/json", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsFile(this HttpContent content)
        {
            return
               content != null &&
               content.Headers != null &&
               content.Headers.ContentType != null &&
               !string.Equals(content.Headers.ContentType.MediaType, "application/json", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
