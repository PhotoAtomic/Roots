using PhotoAtomic.ComposableExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    [Export(typeof(IQueryableAsyncExtensions))]
    [ExtensionMetadata(Provider = typeof(EnumerableQuery))]
    public class QueryableAsyncForEnumerableExtensions : IQueryableAsyncExtensions
    {
        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source)
        {
            return Task.FromResult(source.SingleOrDefault());
        }

        public Task<IList<TSource>> ToListAsync<TSource>(IQueryable<TSource> source)
        {
            return Task.FromResult((IList<TSource>)source.ToList());
        }
    }
}
