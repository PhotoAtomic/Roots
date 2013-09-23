using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.ComposableExtensions
{
    public static class Extensions
    {
        private static Dictionary<Type, object> extensionsCache = new Dictionary<Type, object>();

        //HINT: find a smarter way to cache the already construced mappers, 
        //instead of locking the dictionary that could result in performance peanlties due to the 
        //potential high traffic concentration on this method
        //use a cache that priviledges reads in term of times and locks only in case of writes
        public static ExtensionMapper<T> OfType<T>()
        {
            object mapper = null;
            var mapperType = typeof(T);
            
            lock (extensionsCache)
            {                
                if (extensionsCache.TryGetValue(mapperType, out mapper)) return (ExtensionMapper<T>)mapper;
                mapper = ExtensionMapper<T>.MakeNew();
                extensionsCache.Add(mapperType, mapper);
                return (ExtensionMapper<T>)mapper;
            }

        }

        public static T Of<T>(Type provider)
        {
            return Extensions.OfType<T>().GetMapperByProvider(provider);
        }
    }
}
