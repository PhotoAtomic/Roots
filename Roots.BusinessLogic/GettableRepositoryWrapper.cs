using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    class GettableRepositoryWrapper<T> : IGettableQueryable<T>
    {
        private Persistence.IRepository<T> repository;

        public GettableRepositoryWrapper(Persistence.IRepository<T> repository)
        {            
            this.repository = repository;
        }

        

        public T GetById(object id)
        {
            return repository.GetById(id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return repository.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get { return repository.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return repository.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return repository.Provider; }
        }
    }
}
