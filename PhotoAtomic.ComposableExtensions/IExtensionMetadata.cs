using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace PhotoAtomic.ComposableExtensions
{    
    public interface IExtensionMetadata
    {
        
        Type Provider { get;}
    }
}
