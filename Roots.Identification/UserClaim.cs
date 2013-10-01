using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class UserClaim: IUserClaim
    {
        public string ClaimType
        {
            get;
            set;
        }

        public string ClaimValue
        {
            get;
            set;
        }

        public string UserId
        {
            get;
            set;
        }
    }
}
