using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb
{
    class RavenDbAsyncUnitOfWork : IAsyncUnitOfWork
    {
        private IAsyncDocumentSession documentSession;
        private IDocumentStore documentStore;
        private ICollection<IDisposable> trackedRepositories;
        private IsolationLevel isolationLevel;

        public RavenDbAsyncUnitOfWork(IDocumentStore documentStore, IsolationLevel isolationLevel = IsolationLevel.None)
        {
            trackedRepositories = new List<IDisposable>();
            this.isolationLevel = isolationLevel;
            this.documentStore = documentStore;
            this.documentSession = documentStore.OpenAsyncSession();
        }

        public IAsyncRepository<T> RepositoryOf<T>()
        {
            var newRepository = new RavenDbAsyncRepository<T>(
                documentSession, 
                documentStore.Conventions.GetIdentityProperty, 
                isolationLevel);
            trackedRepositories.Add(newRepository);
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
                repository.Dispose();
            }
            trackedRepositories.Clear();
        }

        
    }
}
