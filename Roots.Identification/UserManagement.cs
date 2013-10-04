using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.Identification
{
    class UserManagement : IUserManagement
    {
        public string UserId 
        { 
            get; 
            set; 
        }

        public bool DisableSignIn
        {
            get;
            set;
        }

        public DateTime LastSignInTimeUtc
        {
            get;
            set;
        }
    }
}
