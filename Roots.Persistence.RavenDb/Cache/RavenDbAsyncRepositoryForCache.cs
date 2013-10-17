using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PhotoAtomic.Extensions;

namespace Roots.Persistence.RavenDb.Cache
{
    public class RavenDbAsyncRepositoryForCache<T> : RavenDbAsyncRepository<T>
    {
        public RavenDbAsyncRepositoryForCache(IAsyncDocumentSession documentSession, Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel = IsolationLevel.None)
            : base(documentSession, getIdentityProperty, isolationLevel)
        {
            wrappedProvider = new RavenDbQueryProviderForCache(base.Provider, Repository);    
        }

        private IQueryProvider wrappedProvider;
        public override IQueryProvider Provider
        {
            get { return wrappedProvider; }
        }
        
    }
}
