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
        string IToken.Id
        {
            get
            {
                return base.Id.ToString();
            }
            set
            {
                base.Id = Guid.Parse(value);
            }
        }

        DateTime IToken.ValidUntilUtc
        {
            get 
            {
                return base.UtcExpireDate;
            }
            set
            {
                base.UtcExpireDate = value;
            }
        }
    }
}
