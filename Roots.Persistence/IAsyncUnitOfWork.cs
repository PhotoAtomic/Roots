using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public interface IAsyncUnitOfWork : IDisposable
    {
        IAsyncRepository<T> RepositoryOf<T>();

        Task CommitAsync();

        Task RollbackAsync(); 
    }
}
