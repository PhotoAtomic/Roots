using PhotoAtomic.ComposableExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    [Export(typeof(IQueryableAsyncExtensions))]
    [ExtensionMetadata(Provider = typeof(MemoryCacheQueryProvider<>))]
    public class QueryableAsyncForMemoryCacheExtensions : IQueryableAsyncExtensions
    {
        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source)
        {            
            return MemoryCacheExtensions.SingleOrDefaultAsync(source);            
        }

        public Task<IList<TSource>> ToListAsync<TSource>(IQueryable<TSource> source)
        {
            return MemoryCacheExtensions.ToObservable(source);         
        }
    }
}
