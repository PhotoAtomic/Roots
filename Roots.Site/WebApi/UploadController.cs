using PhotoAtomic.AspNet.MediaFormatters;
using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;
using Roots.SupportedFileTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Roots.Site.WebApi
{
    public class UploadController : ApiController
    {

        DomainSupervisor domain;

        [ImportingConstructor]
        public UploadController([Import]DomainSupervisor domain)
        {
            this.domain = domain;
        }


        public async void Post(MultiPartBody multiPart)
        {
            var file = multiPart.Get<MultiPartFile>("file");

            string mimeType = file.MimeType;
            if (mimeType == MimeTypes.OctetStream)
            {
                mimeType = MimeTypes.GetFileType(Path.GetExtension(file.Name));
            }

            var fileAdded = new NewFileUploader
            {
                Source = String.Format("User Browser-{0}", Guid.NewGuid()),
                SourceName = file.Name,
                MimeType = mimeType,
                FileContent = await file.AsByteArray(),
            };
            domain.Apply(fileAdded);

        }
    }
}