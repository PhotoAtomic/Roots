using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Linq;
using System.ComponentModel.Composition;
using PhotoAtomic.ComposableExtensions;

namespace Roots.Persistence.RavenDb
{
    [Export(typeof (IQueryableAsyncExtensions))]
    [ExtensionMetadata(Provider=typeof(IRavenQueryProvider))]
    public class RavenDbQueryableAsyncExtensions : IQueryableAsyncExtensions
    {
        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source)
        {
            return Raven.Client.LinqExtensions.SingleOrDefaultAsync(source);            
        }
    }
}
