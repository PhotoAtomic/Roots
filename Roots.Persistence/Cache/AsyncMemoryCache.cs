using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    public class AsyncMemoryCache : IAsyncUnitOfWork
    {
        private IUnitOfWorkFactory factory;

        private IDictionary<Type, IAsyncCachedRepository> cachedRepositories = new Dictionary<Type, IAsyncCachedRepository>();


        public AsyncMemoryCache(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
        }

        public IAsyncRepository<T> RepositoryOf<T>()
        {
            IAsyncCachedRepository repository;
            Type typeOfT = typeof(T);

            if(cachedRepositories.TryGetValue(typeOfT,out repository))return (IAsyncRepository<T>)repository;
            var repo = new AsyncMemoryRepository<T>(factory);
            cachedRepositories.Add(typeOfT, repo);
            return repo;            
        }

        public async Task CommitAsync()
        {
            using (var uow = factory.CreateAsyncNew())
            {
 
                await Task.WhenAll(cachedRepositories.Values.Select(x => x.ApplyAsync(uow)).ToArray());
                await uow.CommitAsync();
            }
            cachedRepositories = new Dictionary<Type, IAsyncCachedRepository>();            
        }

        public Task RollbackAsync()
        {
            cachedRepositories = new Dictionary<Type, IAsyncCachedRepository>();
            return Task.FromResult(true);
        }

        public void Dispose()
        {
            cachedRepositories = null;
        }
    }
}
