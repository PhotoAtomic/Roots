using Roots.Connectors.Interfaces;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roots.Web.Controllers
{
    public class ContentController : ApiController
    {
        IUnitOfWorkFactory factory;


        public ContentController(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
        }

        // GET api/content
        public IEnumerable<string> Get(string root)
        {
            throw new NotImplementedException();
        }

        // GET api/content/5
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/content
        public void Post([FromBody]FileContent fileContent)
        {
            using (var uow = factory.CreateNew())
            {
                var content = uow.RepositoryOf<Domain.FileContent>()
                    .Where(x => 
                        x.Name == fileContent.Destination &&
                        x.MimeType == fileContent.MimeType)
                    .SingleOrDefault();

                if (content == null)
                {
                    content = new Domain.FileContent
                    {
                        Data = fileContent.Content,
                        MimeType = fileContent.MimeType,
                        Name = fileContent.Destination
                    };
                }
                else
                {
                    content.Data = fileContent.Content;                    
                }
                uow.RepositoryOf<Domain.FileContent>().Add(content);
                uow.Commit();
            }
        }

        // PUT api/content/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/content/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
