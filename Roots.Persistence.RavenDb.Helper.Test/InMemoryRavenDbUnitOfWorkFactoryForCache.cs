using Raven.Client.Document;
using Raven.Client.Embedded;
using Roots.Persistence.RavenDb;
using Roots.Persistence.RavenDb.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb.Helper.Test
{
    public class InMemoryRavenDbUnitOfWorkFactoryForCache : RavenDbUnitOfWorkFactoryForMemoryCache
    {
        public InMemoryRavenDbUnitOfWorkFactoryForCache()
            : base(null)
        {

        }

        protected override Raven.Client.IDocumentStore MakeDocumentStore(string connectionString)
        {
            return new EmbeddableDocumentStore() { RunInMemory = true };
        }
    }
}
