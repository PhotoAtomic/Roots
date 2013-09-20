using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public static class QueryableAsyncExtensions
    {

        static private ExtensionMapper<IQueryableAsyncExtensions> mapper;
        
        static QueryableAsyncExtensions()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            assemblies.SelectMany(x => x.GetReferencedAssemblies());


            var catalog = new ApplicationCatalog();
            CompositionContainer container = new CompositionContainer(catalog);
            container.GetExport<ExtensionMapper<IQueryableAsyncExtensions>>();            
        }

        public static void RegisterExtensionMapper(Type providerType, IQueryableAsyncExtensions extensions)
        {
            // consider using Dependency injection resolver
        }

        private static IQueryableAsyncExtensions GetMapping(IQueryable source)
        {

            throw new NotImplementedException();
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source)
        {            
            var mapping = GetMapping(source);
            return mapping.SingleOrDefaultAsync<TSource>(source);
        }
    }
}
