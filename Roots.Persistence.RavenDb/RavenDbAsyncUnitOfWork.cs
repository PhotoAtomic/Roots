using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Roots.Persistence.RavenDb
{
    public class RavenDbAsyncUnitOfWork : IAsyncUnitOfWork
    {
        private IAsyncDocumentSession documentSession;
        private IDocumentStore documentStore;
        private IDictionary<Type,IDisposable> trackedRepositories;
        private IsolationLevel isolationLevel;

        public RavenDbAsyncUnitOfWork(IDocumentStore documentStore, IsolationLevel isolationLevel = IsolationLevel.None)
        {
            trackedRepositories = new Dictionary<Type, IDisposable>();
            this.isolationLevel = isolationLevel;
            this.documentStore = documentStore;
            if (Transaction.Current != null) throw new Exception("RavenDb doesn't support Async in DTC transactions, thanks ayende.");
            this.documentSession = documentStore.OpenAsyncSession();
            if (Transaction.Current != null)
            {
                this.documentSession.Advanced.AllowNonAuthoritativeInformation = false;
            }
        }

        public IAsyncRepository<T> RepositoryOf<T>()
        {
            IDisposable foundRepository;
            if (trackedRepositories.TryGetValue(typeof(T), out foundRepository)) return (IAsyncRepository<T>)foundRepository;
            var newRepository = MakeNewRepository<T>(documentSession, documentStore.Conventions.GetIdentityProperty, isolationLevel);
            trackedRepositories.Add(typeof(T),newRepository);
            return newRepository;
        }

        protected virtual RavenDbAsyncRepository<T> MakeNewRepository<T>(IAsyncDocumentSession documentSession, Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel)
        {
            var newRepository = new RavenDbAsyncRepository<T>(
                documentSession,
                documentStore.Conventions.GetIdentityProperty,
                isolationLevel);
            return newRepository;
        }

        public Task CommitAsync()
        {
            return documentSession.SaveChangesAsync();            
        }

        public Task RollbackAsync()
        {
            DisposeRepositories();
            documentSession.Dispose();
            documentSession = documentStore.OpenAsyncSession();
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            DisposeRepositories();
            documentSession.Dispose();
            documentStore = null;
            documentSession = null;            
        }

        protected void DisposeRepositories()
        {
            if (trackedRepositories == null) return;
            foreach (var repository in trackedRepositories)
            {
                repository.Value.Dispose();
            }
            trackedRepositories.Clear();
        }

        
    }
}
