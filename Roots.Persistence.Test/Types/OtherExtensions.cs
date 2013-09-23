using PhotoAtomic.ComposableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Test.Types
{
    public static class OtherExtensions
    {
        
        public static bool Do<TSource>(this IQueryable<TSource> source)
        {
            return Extensions
                .Of<ISomeOtherExtensions>(source.Provider.GetType())
                .Do();
    
        }
    }
}
