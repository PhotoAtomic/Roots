using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public static IEnumerable<T> AddIfNotPresent<T>(
            this IEnumerable<T> enumerable, 
            T itemToAdd, 
            Func<T, T, bool> comparer = null, 
            Func<T, int> hashGenerator = null)
        {
            return AddIfNotPresent(enumerable,itemToAdd, new ItemEqualityComparer<T>(comparer,hashGenerator));
        }

        public static IEnumerable<T> AddIfNotPresent<T>(
            this IEnumerable<T> enumerable,
            T itemToAdd,
            IEqualityComparer<T> comparer)
        {
            return
                itemToAdd
                .AsEnumerable()
                .Union(enumerable, comparer);

        }

        public static IEnumerable<T> AddIfNotPresent<T>(
            this IEnumerable<T> enumerable,
            T itemToAdd )
        {
            return
                itemToAdd
                .AsEnumerable()
                .Union(enumerable, EqualityComparer<T>.Default);

        }
    }
}
