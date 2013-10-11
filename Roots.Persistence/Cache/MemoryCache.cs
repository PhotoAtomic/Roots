using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    public class MemoryCache : IUnitOfWork
    {
        private IUnitOfWorkFactory factory;

        private IDictionary<Type, ICachedRepository> cachedRepositories = new Dictionary<Type, ICachedRepository>();


        public MemoryCache(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
        }

        public IRepository<T> RepositoryOf<T>()
        {
            ICachedRepository repository;
            Type typeOfT = typeof(T);

            if(cachedRepositories.TryGetValue(typeOfT,out repository))return (IRepository<T>)repository;
            var repo = new MemoryRepository<T>(factory);
            cachedRepositories.Add(typeOfT, repo);
            return repo;            
        }

        public void Commit()
        {
            using (var uow = factory.CreateNew())
            {
                foreach (var repo in cachedRepositories.Values)
                {
                    repo.Apply(uow);                    
                }
                uow.Commit();
            }
            cachedRepositories = new Dictionary<Type, ICachedRepository>();
        }

        public void Rollback()
        {
            cachedRepositories = new Dictionary<Type, ICachedRepository>();
        }

        public void Dispose()
        {
            cachedRepositories = null;
        }
    }
}
