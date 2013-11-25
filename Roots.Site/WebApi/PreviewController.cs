using ImageResizer;
using Roots.BusinessLogic;
using Roots.BusinessLogic.Extractors;
using Roots.SupportedFileTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Roots.Site.WebApi
{
    public class PreviewController : ApiController
    {
        DomainSupervisor domain;

        [ImportingConstructor]
        public PreviewController([Import]DomainSupervisor domain)
        {
            this.domain = domain;
        }

        public HttpResponseMessage Get(Guid id, [FromUri]int? h = null, [FromUri]int? w = null)
        {            
            var getFile = new GetFileContent{Id = id};
            var file = domain.Apply(getFile);

            if (file == null || file.Data == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            byte[] data = null;

            if (file.MimeType == MimeTypes.ImageJPG && h.HasValue && w.HasValue)
            {
                var resizeSettings = new ResizeSettings(h.Value, w.Value, FitMode.Pad, "jpg");
                var memoryStreamIn = new MemoryStream(file.Data);
                var memoryStreamOut = new MemoryStream();
                ImageBuilder.Current.Build(memoryStreamIn, memoryStreamOut, resizeSettings);                
                data = memoryStreamOut.ToArray();
            }
            else
            {
                data = file.Data;
            }

            response.Content = new ByteArrayContent(data);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(file.MimeType);

            return response;
        }
    }
}
