using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return Enumerable.Empty<T>();
            return enumerable;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.GetEnumerator().MoveNext();
        }
    }
}
