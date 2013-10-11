using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public interface IAsyncRepository<T> : IQueryable<T>
    {
        Task<T> GetByIdAsync(object id);

        Task RemoveByIdAsync(object id);

        Task AddAsync(T item);

        Task RemoveAsync(T item);

        PropertyInfo GetIdProperty();
    }
}
