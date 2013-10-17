using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Roots.Persistence.RavenDb.Cache
{
    public class RavenDbAsyncUnitOfWorkForCache : RavenDbAsyncUnitOfWork
    {
        public RavenDbAsyncUnitOfWorkForCache(IDocumentStore documentStore, IsolationLevel isolationLevel = IsolationLevel.None):base(documentStore,isolationLevel)
        {

        }

        protected override RavenDbAsyncRepository<T> MakeNewRepository<T>(IAsyncDocumentSession documentSession, Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel)
        {
            return new RavenDbAsyncRepositoryForCache<T>(documentSession, getIdentityProperty, isolationLevel);
        }        
    }
}
