using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class UserLogin //: IUserLogin
    {
        public UserLogin()
        {
        }

        public UserLogin(string userId, Domain.Login login)
        {
            UserId = userId;
            LoginProvider = login.Provider;
            ProviderKey = login.ProviderKey;
        }
        public string LoginProvider
        {
            get;
            set;
        }

        public string ProviderKey
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
