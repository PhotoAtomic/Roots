using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.ComposableExtensions
{
    [MetadataAttribute]
    public class ExtensionMetadataAttribute :Attribute, IExtensionMetadata
    {
        public Type Provider
        {
            get;
            set;
        }
    }
}
