using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Persistence.RavenDb
{
    class RavenDbUnitOfWork : IUnitOfWork
    {
        private IDocumentSession documentSession;
        private IDocumentStore documentStore;
        private ICollection<IDisposable> trackedRepositories;

        public RavenDbUnitOfWork(IDocumentStore documentStore)
        {
            trackedRepositories = new List<IDisposable>();
            this.documentStore = documentStore;
            this.documentSession = documentStore.OpenSession();
        }

        public IRepository<T> RepositoryOf<T>()
        {
            var newRepository = new RavenDbRepository<T>(documentSession);
            trackedRepositories.Add(newRepository);
            return newRepository;
        }

        public void Commit()
        {
            documentSession.SaveChanges();            
        }

        public void Rollback()
        {
            DisposeRepositories();
            documentSession.Dispose();
            documentSession = documentStore.OpenSession();
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
