using PhotoAtomic.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Roots.Persistence.Cache
{
    public class MemoryRepository<T> : MemoryRepositoryBase<T>, IRepository<T>, ICachedRepository
    {
        
        
        

        public MemoryRepository(IUnitOfWorkFactory factory) : base(factory)
        {
            
            
        }



        public T GetById(object id)
        {
            AggregateTrackedItem();
            T item;

            object convertedId;
            if(!TryConvertId(id, out convertedId)) return default(T);

            if (cache.TryGetValue(convertedId, out item)) return item;
            using (var uow = factory.CreateNew())
            {
                item = uow.RepositoryOf<T>().GetById(convertedId);                
            }
            if (item == null) return default(T);
            cache[convertedId] = item;
            return item;
        }



        public void Add(T item)
        {
            AggregateTrackedItem();
            if (item == null) throw new ArgumentNullException("item");

            SetIdIfMissing(item);

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




        public void RemoveById(object id)
        {
            AggregateTrackedItem();

            object convertedId;
            if (!TryConvertId(id, out convertedId)) return;

            cache.Remove(convertedId);
            idToRemove.Add(convertedId);
        }



        public override IIdGenerator GetIdGenerator()
        {
            return factory.CreateNew().RepositoryOf<T>() as IIdGenerator;
        }
    }
}
