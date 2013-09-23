using PhotoAtomic.ComposableExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Test.Types
{
    [Export (typeof(IQueryableAsyncExtensions))]
    [ExtensionMetadata(Provider=typeof(EnumerableQuery<int>))]
    class TestExtensionForQueryableAsyncInt : IQueryableAsyncExtensions
    {

        public Task<TSource> SingleOrDefaultAsync<TSource>(IQueryable<TSource> source)
        {
            CalledOn = source;
            return null;
        }

        public static IQueryable CalledOn { get; set; }
    }
}
