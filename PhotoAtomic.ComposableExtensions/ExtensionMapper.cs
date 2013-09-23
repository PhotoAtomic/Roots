using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PhotoAtomic.Extensions;
using System.ComponentModel.Composition.Hosting;

namespace PhotoAtomic.ComposableExtensions
{
    [Export]
    public class ExtensionMapper<TExtension>
    {

        private class AssemblyNetNode
        {
            public AssemblyNetNode(Assembly baseAssembly)
            {
                Base = baseAssembly;
                Derived = new HashSet<AssemblyNetNode>();
            }
            public Assembly Base { get; set; }
            public ISet<AssemblyNetNode> Derived { get; private set; }
        }

        private static object syncLock = new object();
        private static CompositionContainer container;

        public static ExtensionMapper<TExtension> MakeNew()
        {
            lock (syncLock)
            {
                if (container == null)
                {
                    var baseAssembly = typeof(TExtension).Assembly.FullName;

                    var reverseNodesDictionay = ReverseAssemblyDependanceTree();

                    var dependentAssemblies = reverseNodesDictionay[baseAssembly]
                        .FlatHierarchy(x => x.Derived)
                        .Select(x => x.Base)
                        .Union(typeof(ExtensionMapper<>).Assembly.AsEnumerable())
                        .Distinct();
                        
                    var catalogs = new AggregateCatalog(dependentAssemblies.Select(x => new AssemblyCatalog(x)));
                    
                    container = new CompositionContainer(catalogs);
                }
                return container.GetExportedValue<ExtensionMapper<TExtension>>();
            }
        }

        private static Dictionary<string, AssemblyNetNode> ReverseAssemblyDependanceTree()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var nodes = assemblies
                .Select(x =>
                    new
                    {
                        Name = x.FullName,
                        Assembly = x,
                        ReferencesName = x.GetReferencedAssemblies().Select(y => y.FullName)
                    });

            var nodesDictionay = nodes.ToDictionary(x => x.Name);

            var reverseNodesDictionay = nodes.ToDictionary(x => x.Name, x => new AssemblyNetNode(x.Assembly));


            foreach (var node in nodes)
            {
                foreach (var referencedNode in node.ReferencesName)
                {
                    AssemblyNetNode baseNode;
                    if (reverseNodesDictionay.TryGetValue(referencedNode, out baseNode))
                    {
                        baseNode.Derived.Add(reverseNodesDictionay[node.Name]);
                    }
                }
            }
            return reverseNodesDictionay;
        }

        public IEnumerable<Lazy<TExtension, IExtensionMetadata>> Extensions { get; private set; }

        [ImportingConstructor]
        public ExtensionMapper([ImportMany]  IEnumerable<Lazy<TExtension, IExtensionMetadata>> extensions)
        {
            Extensions = extensions;
        }

        public TExtension GetMapperByProvider(Type providerType) 
        {
            return Extensions.Where(x => x.Metadata.Provider == providerType).SingleOrDefault().Value;
        }


    }
}
