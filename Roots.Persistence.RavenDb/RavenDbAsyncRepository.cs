using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb
{
    public class RavenDbAsyncRepository<T> : RavenDbRespositoryBase<T>, IAsyncRepository<T>, IDisposable
    {
        private IAsyncDocumentSession documentSession;
               
        public RavenDbAsyncRepository(IAsyncDocumentSession documentSession, Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel = IsolationLevel.None)
            : base(getIdentityProperty, isolationLevel)
        {
            this.documentSession = documentSession;
            
        }


        public Task<T> GetByIdAsync(object id)
        {
            if (id == null) throw new ArgumentNullException("id");

            return InvokeByTargetType(id, 
                x => documentSession.LoadAsync<T>(x), 
                x => documentSession.LoadAsync<T>(x));
            
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

        public virtual IEnumerator<T> GetEnumerator()
        {
            return Repository.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual Type ElementType
        {
            get { return Repository.ElementType; }
        }

        public virtual System.Linq.Expressions.Expression Expression
        {
            get { return Repository.Expression; }
        }

        public virtual IQueryProvider Provider
        {
            get { return Repository.Provider; }
        }

        public void Dispose()
        {         
            documentSession = null;
        }

        protected override IRavenQueryable<T> GetRavenQueryable()
        {
            return documentSession.Query<T>();
        }
        


        public Task RemoveByIdAsync(object id)
        {
            //HINT: shitty raven, i have to instanciate a type and fill t's id property to obrain its string id
            return Task.Run(() =>
            {
                var key = KeyForId(documentSession.Advanced.DocumentStore, id);
                documentSession.Advanced.Defer(new DeleteCommandData { Key = key });
            });
            
        }

        
    }
}
