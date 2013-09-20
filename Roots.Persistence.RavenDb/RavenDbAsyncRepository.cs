using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb
{
    public class RavenDbAsyncRepository<T> : IAsyncRepository<T>, IDisposable
    {
        private IAsyncDocumentSession documentSession;
        private IRavenQueryable<T> repository;
        public RavenDbAsyncRepository(IAsyncDocumentSession documentSession)
        {
            this.documentSession = documentSession;            
            this.repository = documentSession.Query<T>();
        }

        public Task<T> GetByIdAsync(ValueType id)
        {
            return documentSession.LoadAsync<T>(id);
        }

        public Task<T> GetByIdAsync(string id)
        {
            return documentSession.LoadAsync<T>(id);
        }

        public Task AddAsync(T item)
        {
            return documentSession.StoreAsync(item);
        }

        public Task RemoveAsync(T item)
        {
            documentSession.Delete<T>(item);
            return Task.FromResult<object>(null);
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
