using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Persistence
{


    public interface IExtensionMetadata
    {
        
        Type Provider { get; set; }
    }
}
