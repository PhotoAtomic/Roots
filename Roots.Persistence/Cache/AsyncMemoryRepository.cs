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
            if (cache.TryGetValue(id, out item)) return item;
            using (var uow = factory.CreateAsyncNew())
            {
                item = await uow.RepositoryOf<T>().GetByIdAsync(id);
            }
            if (item == null) return default(T);
            cache[id] = item;
            return item;
        }

        public Task RemoveByIdAsync(object id)
        {
            AggregateTrackedItem();
            cache.Remove(id);
            idToRemove.Add(id);
            return Task.FromResult(true);
        }

        public Task AddAsync(T item)
        {
            AggregateTrackedItem();
            if (item == null) throw new ArgumentNullException("item");
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

    }
}
