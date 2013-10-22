using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using PhotoAtomic.Reflection;

namespace Roots.Persistence.Cache
{
    public abstract class MemoryRepositoryBase<T>: IQueryable<T>
    {
        protected internal IDictionary<object, T> cache = new Dictionary<object, T>();
        protected internal IDictionary<object, T> track = new Dictionary<object, T>();
        protected internal ISet<object> idToRemove = new HashSet<object>();
        protected readonly IUnitOfWorkFactory factory;
        private PropertyInfo idProperty;        

        public MemoryRepositoryBase(IUnitOfWorkFactory factory)
        {
            Expression = Expression.Constant(this);
            Provider = new MemoryCacheQueryProvider<T>();
            this.factory = factory;
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

        protected internal IQueryable<T> GetCacheAsQueryable()
        {
            AggregateTrackedItem();
            return cache.Values.AsQueryable<T>();
        }

        protected internal virtual Tuple<IDisposable, IQueryable<T>> GetRepositoryAsQueryable()
        {
            var uow = factory.CreateNew();
            return Tuple.Create((IDisposable)uow, (IQueryable<T>)uow.RepositoryOf<T>());
        }

        protected internal void AggregateTrackedItem()
        {
            if (track.Count == 0) return;
            cache = cache.Union(track).ToDictionary(x => x.Key, x => x.Value);
            track.Clear();
        }

        protected object GetIdValue(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            return GetIdProperty().GetValue(item);
        }

        protected bool TryConvertId(object id, out object convertedId)
        {
            return Conversion.Try(id, id.GetType(), GetIdProperty().PropertyType, out convertedId);
        }

        internal void Track(T item)
        {
            if (item == null) return;
            object id = GetIdValue(item);
            track[id] = item;
        }

        protected void SetIdIfMissing(T item)
        {
            using (var generator = GetIdGenerator())
            {
                if(generator == null) return;

                var idProperty = GetIdProperty();

                var value = idProperty.GetValue(item);

                if (value!=null && !Activator.CreateInstance(idProperty.PropertyType).Equals(value)) return;

                object id = generator.GenerateNewId();
                if(id == null) return;
                

                if(!Conversion.Try(id, id.GetType(), idProperty.PropertyType, out id)) return;
                idProperty.SetValue(item, id);
            }
        }

        public abstract IIdGenerator GetIdGenerator();
        

        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        
    }
}
