using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb.Cache
{
    public class RavenDbUnitOfWorkFactoryWrapperForMemoryCache : IUnitOfWorkFactory
    {
        private RavenDbUnitOfWorkFactory factory;

        public RavenDbUnitOfWorkFactoryWrapperForMemoryCache(RavenDbUnitOfWorkFactory factory)
        {
            this.factory = factory;
        }

        public IUnitOfWork CreateNew(IsolationLevel isolationLevel = IsolationLevel.None)
        {
            return factory.CreateNew(isolationLevel);
        }

        public IAsyncUnitOfWork CreateAsyncNew(IsolationLevel isolationLevel = IsolationLevel.None)
        {
            return new RavenDbAsyncUnitOfWorkForCache(factory.documentStore, isolationLevel);            
        }
    }
}
