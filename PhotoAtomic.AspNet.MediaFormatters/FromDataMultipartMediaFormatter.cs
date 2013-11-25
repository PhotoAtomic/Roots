using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Validation;

namespace PhotoAtomic.AspNet.MediaFormatters
{
    public class FromDataMultipartMediaFormatter : MediaTypeFormatter
    {
        private JsonSerializerSettings settings;


        public FromDataMultipartMediaFormatter()
        {

            settings = new JsonSerializerSettings();
            settings.ContractResolver = new JsonContractResolver(this);
            settings.MissingMemberHandling = 0;
            settings.TypeNameHandling = 0;


            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));            
        }
        public override bool CanReadType(Type type)
        {
            return type == typeof(MultiPartBody);
        }
        public override bool CanWriteType(Type type)
        {
            return false;
        }

        async public override Task<object> ReadFromStreamAsync(Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {            
            
            if (!content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var multiPart = await content.ReadAsMultipartAsync();

            
            

            var body = new MultiPartBody();

            foreach(var part in multiPart.Contents){
                var partName = part.Headers.ContentDisposition.Name.Replace("\"",string.Empty);

                if(part.IsJson())
                {

                    Encoding encoding = base.SelectCharacterEncoding(part.Headers);

                    var json = await part.ReadAsStreamAsync();
                    body.Add(
                        partName,
                        new MultiPartJson(json, encoding, settings));

                }
                else if (part.IsFile())
                {
                    var mimeType = part.Headers.ContentType.MediaType;
                    var stream = await part.ReadAsStreamAsync();
                    var fileName = part.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    body.Add(
                        partName,
                        new MultiPartFile(fileName, mimeType, stream));
                }
                else
                {
                    var text = await part.ReadAsStringAsync();
                    body.Add(partName, text);
                }
            }
            
            return body;                      
        }
    }
}
