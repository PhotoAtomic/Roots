using PhotoAtomic.ComposableExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Test.Types
{
    [Export(typeof(ISomeOtherExtensions))]
    [ExtensionMetadata(Provider = typeof(EnumerableQuery<int>))]
    public class SomeOtherExtension : ISomeOtherExtensions
    {
        public bool Do()
        {
            return true;
        }
    }
}
