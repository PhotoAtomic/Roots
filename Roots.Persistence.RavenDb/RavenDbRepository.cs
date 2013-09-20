using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Persistence.RavenDb
{
    class RavenDbRepository<T> : IRepository<T>, IDisposable
    {
        private IDocumentSession documentSession;
        private IRavenQueryable<T> repository;
        public RavenDbRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
            this.repository = documentSession.Query<T>();
        }

        public T GetById(ValueType id)
        {
            return documentSession.Load<T>(id);
        }
        public T GetById(string id)
        {
            return documentSession.Load<T>(id);
        }

        public void Add(T item)
        {
            documentSession.Store(item);
        }

        public void Remove(T item)
        {
            documentSession.Delete<T>(item);
        }

        public IEnumerator<T> GetEnumerator()
        {            
            return repository.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)repository).GetEnumerator();
        }

        public Type ElementType
        {
            get { return repository.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return repository.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return repository.Provider; }
        }

        public void Dispose()
        {
            repository = null;
            documentSession = null;
        }
    }
}
