using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    public interface IAsyncCachedRepository
    {
        Task ApplyAsync(IAsyncUnitOfWork uow);
    }
}
