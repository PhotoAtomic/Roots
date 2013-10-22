using Roots.Persistence;
using Roots.Persistence.RavenDb.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Roots.Web.Parts
{
    [Export("Roots:cacheFactory", typeof(IUnitOfWorkFactory))]
    public class UnitOfWorkFactoryForCachePart : IUnitOfWorkFactory
    {
        private RavenDbUnitOfWorkFactoryWrapperForMemoryCache cacheFactory;

        [ImportingConstructor]
        public UnitOfWorkFactoryForCachePart(UnitOfWorkFactoryPart factoryPart)
        {
            cacheFactory = new RavenDbUnitOfWorkFactoryWrapperForMemoryCache(factoryPart.factory);
        }

        public IUnitOfWork CreateNew(IsolationLevel isolationLevel)
        {
            return cacheFactory.CreateNew(IsolationLevel.ReadItsOwnWrite);
            
        }

        public IAsyncUnitOfWork CreateAsyncNew(IsolationLevel isolationLevel)
        {
            return cacheFactory.CreateAsyncNew(IsolationLevel.ReadItsOwnWrite);
            
        }
    }
}