using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T root)
        {
            yield return root;
        }

        public static IEnumerable<T> Flat<T>(this T root, Func<T, T> leaf)
        {
            return FlatHierarchy(root, x => leaf(x).AsEnumerable());
        }

        public static IEnumerable<T> FlatHierarchy<T>(this T root, Func<T, IEnumerable<T>> getLeafs)
        {
            if (root == null)
                yield break;

            yield return root;

            foreach (var leaf in getLeafs(root))
                foreach (var flat in leaf.FlatHierarchy<T>(getLeafs))
                    yield return flat;

        }
    }
}
