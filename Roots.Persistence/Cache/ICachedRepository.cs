using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Persistence.Cache
{
    public interface ICachedRepository
    {
        void Apply(IUnitOfWork uow);
    }
}
