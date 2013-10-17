using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb.Cache
{
    public class RavenDbUnitOfWorkFactoryForMemoryCache : RavenDbUnitOfWorkFactory
    {
        public RavenDbUnitOfWorkFactoryForMemoryCache(string connectionString)
            : base(connectionString)
        {
        }

        public override IAsyncUnitOfWork CreateAsyncNew(IsolationLevel isolationLevel = IsolationLevel.None)
        {
            return new RavenDbAsyncUnitOfWorkForCache(documentStore, isolationLevel);
        }
    }
}
