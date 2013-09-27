﻿using Raven.Client;
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

        public IEnumerator<T> GetEnumerator()
        {
            return Repository.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Repository).GetEnumerator();
        }

        public Type ElementType
        {
            get { return Repository.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return Repository.Expression; }
        }

        public IQueryProvider Provider
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
    }
}
