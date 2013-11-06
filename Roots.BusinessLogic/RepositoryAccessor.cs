using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public class RepositoryAccessor
    {
        private Persistence.IUnitOfWork uow;

        public RepositoryAccessor(Persistence.IUnitOfWork uow)
        {            
            this.uow = uow;
        }

        public IRepository<T> RepositoryOf<T>()
        {
            return uow.RepositoryOf<T>();
        }
    }
}
