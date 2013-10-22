using PhotoAtomic.Reflection;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Roots.Persistence.RavenDb
{
    class RavenDbRepository<T> : RavenDbRespositoryBase<T>, IRepository<T>, IDisposable
    {
        private IDocumentSession documentSession;
        
        public RavenDbRepository(IDocumentSession documentSession, Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel = IsolationLevel.None)
            : base(documentSession.Advanced.DocumentStore, getIdentityProperty, isolationLevel)
        {
            
            this.documentSession = documentSession;            
        }

        public T GetById(object id)
        {
            if (id == null) throw new ArgumentNullException("id");

            return InvokeByTargetType(id,
                x => documentSession.Load<T>(x),
                x => documentSession.Load<T>(x));
        }

        public void Add(T item)
        {
            documentSession.Store(item);
        }

        public void Remove(T item)
        {                        
            documentSession.Delete<T>(item);
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

        public override void Dispose()
        {            
            documentSession = null;
        }

        protected override IRavenQueryable<T> GetRavenQueryable()
        {
            return documentSession.Query<T>();
        }


        public void RemoveById(object id)
        {
            var key = KeyForId(id);
            documentSession.Advanced.Defer(new DeleteCommandData { Key = key });
        }
       
    }
}
