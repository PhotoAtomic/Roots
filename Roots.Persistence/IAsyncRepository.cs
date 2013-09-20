using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public interface IAsyncRepository<T> : IQueryable<T>
    {
        Task<T> GetByIdAsync(ValueType id);

        Task<T> GetByIdAsync(string id);

        Task AddAsync(T item);

        Task RemoveAsync(T item);     
    }
}
