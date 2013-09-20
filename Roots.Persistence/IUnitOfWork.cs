using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> RepositoryOf<T>();

        void Commit();

        void Rollback();        
    }
}
