using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Roots.Persistence.Cache
{
    public class MemoryRepository<T> : IRepository<T>, ICachedRepository
    {
        protected internal IDictionary<object,T> cache = new Dictionary<object,T>();
        protected internal IDictionary<object, T> track = new Dictionary<object, T>();
        private ISet<object> idToRemove = new HashSet<object>();
        private IUnitOfWorkFactory factory;
        private PropertyInfo idProperty;
        
        

        public MemoryRepository(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
            Expression = Expression.Constant(this);
            Provider = new MemoryCacheQueryProvider<T>();
        }

        public MemoryRepository(IUnitOfWorkFactory factory, Expression expression, IQueryProvider provider)
        {
            this.factory = factory;
            Expression = expression;
            Provider = provider;
        }

        public T GetById(object id)
        {
            AggregateTrackedItem();
            T item;
            if (cache.TryGetValue(id, out item)) return item;
            using (var uow = factory.CreateNew())
            {
                item = uow.RepositoryOf<T>().GetById(id);                
            }
            if (item == null) return default(T);
            cache[id] = item;
            return item;
        }

        protected internal void AggregateTrackedItem()
        {
            if (track.Count == 0) return;
            cache = cache.Union(track).ToDictionary(x=>x.Key,x=>x.Value);
            track.Clear();
        }

        public void Add(T item)
        {
            AggregateTrackedItem();
            if (item == null) throw new ArgumentNullException("item");
            var id = GetIdValue(item);
            cache[id] = item;
        }

        public void Remove(T item)
        {
            AggregateTrackedItem();
            if (item == null) throw new ArgumentNullException("item");
            var id = GetIdValue(item);
            cache.Remove(id);            
            idToRemove.Add(id);
        }

        public IEnumerator<T> GetEnumerator()
        {            
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get;
            private set;
        }

        public IQueryProvider Provider
        {
            get;
            private set;
        }

        public void Apply(IUnitOfWork uow)
        {
            AggregateTrackedItem();

            var repo = uow.RepositoryOf<T>();

            foreach (var item in cache.Values)
            {
                repo.Add(item);               
            }
            foreach (var id in idToRemove)
            {
                repo.RemoveById(id);
            }
        }


        public PropertyInfo GetIdProperty()
        {

            if (idProperty != null) return idProperty;
            //HINT: hmm, this doesn't looks good. create a uow just to know the id property is overwelming
            //but anyway it is correct that the IdProperty is returned by the repostitory
            //as conceptually it is an it's own property
            using (var uow = factory.CreateNew())
            {
                idProperty = uow.RepositoryOf<T>().GetIdProperty();
            }
            return idProperty;
        }

        protected object GetIdValue(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            return GetIdProperty().GetValue(item);
        }

        internal void Track(T item)
        {
            if (item == null) return;
            object id = GetIdValue(item);
            track[id] = item;
        }

        public void RemoveById(object id)
        {
            AggregateTrackedItem();            
            cache.Remove(id);
            idToRemove.Add(id);
        }

        protected internal IQueryable<T> GetCacheAsQueryable()
        {
            AggregateTrackedItem();
            return cache.Values.AsQueryable<T>();
        }

        protected internal Tuple<IUnitOfWork,IQueryable<T>> GetRepositoryAsQueryable()
        {
            var uow = factory.CreateNew();
            return Tuple.Create(uow, (IQueryable<T>)uow.RepositoryOf<T>());            
        }
    }
}
