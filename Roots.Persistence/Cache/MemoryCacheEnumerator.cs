using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoAtomic.Extensions;


namespace Roots.Persistence.Cache
{
    class MemoryCacheEnumerator<T> : IEnumerator<T>
    {
        private MemoryRepository<T> memoryRepository;
        private IUnitOfWorkFactory factory;
        private IUnitOfWork uow;
        public IEnumerator<T> internalEnumerator;

        public MemoryCacheEnumerator(MemoryRepository<T> memoryRepository, IUnitOfWorkFactory factory)
        {   
            this.memoryRepository = memoryRepository;
            this.factory = factory;

            var idProperty = memoryRepository.GetIdProperty();

            uow = factory.CreateNew();
            internalEnumerator =
                memoryRepository.cache.Values
                .Union(
                    uow.RepositoryOf<T>(),
                    new ItemEqualityComparer<T>(
                        (x, y) => idProperty.GetValue(x) == idProperty.GetValue(y),
                        x =>
                        {
                            var value = idProperty.GetValue(x);
                            if (value == null) return 0;
                            return value.GetHashCode();
                        }))
                .GetEnumerator();

        }

        public T Current
        {
            get
            {
                return internalEnumerator.Current;
            }
            
        }

        public void Dispose()
        {
            uow.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {            
            var hasNext = internalEnumerator.MoveNext();
            if (!hasNext)
            {             
                return false;
            }
            memoryRepository.Track(internalEnumerator.Current);
            return hasNext;
        }

        public void Reset()
        {
            internalEnumerator.Reset();
        }

        
    }
}
