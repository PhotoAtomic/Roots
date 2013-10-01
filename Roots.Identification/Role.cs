using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class Role : IRole
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
