using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class Token : Domain.SecurityToken, IToken
    {
        public string Id
        {
            get
            {
                return Guid.ToString();
            }
            set
            {
                Guid = Guid.Parse(value);
            }
        }


        public Guid Guid
        {
            get;
            set;
        }

        DateTime IToken.ValidUntilUtc
        {
            get;
            set;
        }
    }
}
