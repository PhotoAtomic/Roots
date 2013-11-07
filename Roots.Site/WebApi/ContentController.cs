using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;
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

        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public void Post([FromBody]FileContent value)
        {
            var fileAdded = new NewFileUploaded
            {
                MimeType = value.MimeType,
                Name = value.Destination,
                FileContent = value.Content,
            };
            domain.Apply(fileAdded);
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]FileContent value)
        {
            var fileUpdated = new ExistingFileUpdated
            {
                Name = id,
                MimeType = value.MimeType,
                NewName = value.Destination,
                FileContent = value.Content,
            };
            try
            {
                domain.Apply(fileUpdated);                
            }
            catch (FileNotFoundException)
            {
                var fileAdded = new NewFileUploaded
                {
                    MimeType = value.MimeType,
                    Name = value.Destination,
                    FileContent = value.Content,
                };
                domain.Apply(fileAdded);
            }
        }

        public void Put(string id, string newName)
        {
            var fileUpdated = new ExistingFileRenamed
            {
                OldName = id,
                NewName = newName,
            };
            try
            {
                domain.Apply(fileUpdated);                
            }
            catch (Exception)
            {                
            }
        }

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}