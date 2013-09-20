using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence
{
    public class ExtensionMapper<TExtension>
    {        
        public IEnumerable<Lazy<TExtension, IExtensionMetadata>> Extensions { get; private set; }

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
