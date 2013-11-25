using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public class ReadOnlyRepositoryAccessor
    {
        private Persistence.IUnitOfWork uow;

        public ReadOnlyRepositoryAccessor(Persistence.IUnitOfWork uow)
        {            
            this.uow = uow;
        }

        public IGettableQueryable<T> RepositoryOf<T>()
        {
            return new GettableRepositoryWrapper<T>(uow.RepositoryOf<T>());
        }
    }
}
