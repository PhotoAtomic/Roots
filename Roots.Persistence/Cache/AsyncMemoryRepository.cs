using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    public class AsyncMemoryRepository<T> : MemoryRepositoryBase<T>, IAsyncRepository<T>, IAsyncCachedRepository
    {




        public AsyncMemoryRepository(IUnitOfWorkFactory factory)
            : base(factory)
        {
            
            
        }
        

        public Task ApplyAsync(IAsyncUnitOfWork uow)
        {
            AggregateTrackedItem();

            var repo = uow.RepositoryOf<T>();


            var tasks = cache.Values.Select(x => repo.AddAsync(x))
            .Union(idToRemove.Select(x => repo.RemoveByIdAsync(x)));

            return Task.WhenAll(tasks.ToArray());            
        }


        protected internal override Tuple<IDisposable, IQueryable<T>> GetRepositoryAsQueryable()
        {
            var uow = factory.CreateAsyncNew();
            return Tuple.Create((IDisposable)uow, (IQueryable<T>)uow.RepositoryOf<T>());
        }


        public async Task<T> GetByIdAsync(object id)
        {
            AggregateTrackedItem();
            T item;

            object convertedId;
            if (!TryConvertId(id, out convertedId)) return default(T);

            if (cache.TryGetValue(convertedId, out item)) return item;
            using (var uow = factory.CreateAsyncNew())
            {
                item = await uow.RepositoryOf<T>().GetByIdAsync(convertedId);
            }
            if (item == null) return default(T);
            cache[convertedId] = item;
            return item;
        }

        public Task RemoveByIdAsync(object id)
        {
            AggregateTrackedItem();

            object convertedId;
            if (!TryConvertId(id, out convertedId)) return Task.FromResult(false);

            cache.Remove(convertedId);
            idToRemove.Add(convertedId);
            return Task.FromResult(true);
        }

        public Task AddAsync(T item)
        {
            AggregateTrackedItem();
            if (item == null) throw new ArgumentNullException("item");

            SetIdIfMissing(item);

            var id = GetIdValue(item);
            cache[id] = item;
            return Task.FromResult(true);
        }

        public Task RemoveAsync(T item)
        {
            AggregateTrackedItem();
            if (item == null) throw new ArgumentNullException("item");
            var id = GetIdValue(item);
            cache.Remove(id);
            idToRemove.Add(id);
            return Task.FromResult(true);
        }


        public override IIdGenerator GetIdGenerator()
        {
            
            return factory.CreateAsyncNew().RepositoryOf<T>() as IIdGenerator;
            
        }
    }
}
