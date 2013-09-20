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

        public RavenDbAsyncUnitOfWork(IDocumentStore documentStore)
        {
            trackedRepositories = new List<IDisposable>();
            this.documentStore = documentStore;
            this.documentSession = documentStore.OpenAsyncSession();
        }

        public IAsyncRepository<T> RepositoryOf<T>()
        {
            var newRepository = new RavenDbAsyncRepository<T>(documentSession);
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
