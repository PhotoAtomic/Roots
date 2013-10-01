using PhotoAtomic.ComposableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public static class QueryableAsyncExtensions
    {
        
        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source)
        {
            return Extensions
                .Of<IQueryableAsyncExtensions>(source.Provider.GetType())
                .SingleOrDefaultAsync<TSource>(source);
        }

        public static Task<IList<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source)
        {
            return Extensions
                .Of<IQueryableAsyncExtensions>(source.Provider.GetType())
                .ToListAsync<TSource>(source);
        }
    }
}
