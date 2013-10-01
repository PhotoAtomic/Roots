using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public interface IQueryableAsyncExtensions
    {
        Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source);

        Task<IList<TSource>> ToListAsync<TSource>(IQueryable<TSource> source);
    }
}
