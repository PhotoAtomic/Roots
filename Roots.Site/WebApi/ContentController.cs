using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;
using Roots.BusinessLogic.Extractors;
using Roots.Connectors.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roots.Site.WebApi
{
    public class ContentController : ApiController
    {

        DomainSupervisor domain;

        [ImportingConstructor]
        public ContentController([Import]DomainSupervisor domain)
        {
            this.domain = domain;
        }

        
        public IEnumerable<string> Get(string source, string path)
        {            
            var fileInSource = new GetAllFileInSourcePath
            {
                Source = source,
                Path = path,
            };
            var files = domain.Apply(fileInSource);
            return files;
        }


        public void Post(string source, string id, [FromBody]FileContent value, bool requireApproval = false)
        {
            if (!requireApproval)
            {
                var fileAdded = new NewFileUploader
                {
                    Source = source,
                    SourceName = id,
                    MimeType = value.MimeType,
                    FileContent = value.Content,
                };
                domain.Apply(fileAdded);
            }
            else
            {
                var fileToApprove = new NewFileToApproveUploader
                {
                    Source = source,
                    SourceName = id,
                    MimeType = value.MimeType,
                    FileContent = value.Content,
                };
            }
        }

        
        public void Put(string source, string id, [FromBody]FileContent value)
        {
            var fileUpdated = new ExistingFileContentUpdated
            {
                Source = source,
                SourceName = id,
                MimeType = value.MimeType,                
                FileContent = value.Content,
            };
            try
            {
                domain.Apply(fileUpdated);                
            }
            catch (FileNotFoundException)
            {
                var fileAdded = new NewFileUploader
                {
                    Source = source,
                    SourceName = id,
                    MimeType = value.MimeType,                    
                    FileContent = value.Content,
                };
                domain.Apply(fileAdded);
            }
        }

        public void Put(string source, string id, string newName)
        {
            var fileUpdated = new ExistingFileRenamer
            {
                Source  = source,
                OldSourceName = id,
                NewSourceName = newName,
            };
            try
            {
                domain.Apply(fileUpdated);                
            }
            catch (Exception)
            {                
            }
        }
        
        public void Delete(string source, string id)
        {
            var fileRemoved = new FileRemover
            {     
                Source = source,
                SourceName = id,             
            };
            domain.Apply(fileRemoved);
        }
    }
}