using Roots.BusinessLogic;
using Roots.BusinessLogic.Mutators;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roots.Site.WebApi
{
    public class TypeController : ApiController
    {


        DomainSupervisor domain;

        [ImportingConstructor]
        public TypeController([Import]DomainSupervisor domain)
        {
            this.domain = domain;
        }

        public void Put(string source, string id, string newType)
        {

            var typeUpdated = new ExistingFileTypeChanger
            {
                Source = source,
                SourceName = id,
                NewType = newType,
            };
            try
            {
                domain.Apply(typeUpdated);
            }
            catch (Exception)
            {
            }
        }
    }
}