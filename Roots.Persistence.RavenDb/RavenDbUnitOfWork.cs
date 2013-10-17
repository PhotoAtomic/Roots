using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;

namespace Roots.Persistence.RavenDb
{
    class RavenDbUnitOfWork : IUnitOfWork
    {
        private IDocumentSession documentSession;
        private IDocumentStore documentStore;
        private IDictionary<Type,IDisposable> trackedRepositories;
        private IsolationLevel isolationLevel;

        public RavenDbUnitOfWork(IDocumentStore documentStore, IsolationLevel isolationLevel = IsolationLevel.None)
        {
            trackedRepositories = new Dictionary<Type, IDisposable>();
            this.documentStore = documentStore;
            this.isolationLevel = isolationLevel;
            this.documentSession = documentStore.OpenSession(); //HINT: make Lazy (also on the async version)
            if (Transaction.Current != null)
            {
                this.documentSession.Advanced.AllowNonAuthoritativeInformation = false;
            }
        }

        public IRepository<T> RepositoryOf<T>()
        {
            IDisposable foundRepository;
            if (trackedRepositories.TryGetValue(typeof(T), out foundRepository)) return (IRepository<T>)foundRepository;
            var newRepository = MakeNewRepository<T>(documentSession,documentStore.Conventions.GetIdentityProperty,isolationLevel);
            trackedRepositories.Add(typeof(T),newRepository);
            return newRepository;
        }

        protected virtual RavenDbRepository<T> MakeNewRepository<T>(IDocumentSession documentSession, Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel)
        {
            var newRepository = new RavenDbRepository<T>(
                documentSession,
                documentStore.Conventions.GetIdentityProperty,
                isolationLevel);
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
                repository.Value.Dispose();
            }
            trackedRepositories.Clear();
        }

        
    }
}
