using Roots.Persistence.RavenDb.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Domain
{    
    public class Login
    {        
        public string Provider { get; set; }
        
        public string ProviderKey { get; set; }
    }
}
