using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class User : IUser
    {
        
        public User()
        {

        }

        public User(string userName)
        {
            // TODO: Complete member initialization
            UserName = userName;
        }

        public User(Domain.User user)
        {
            this.Guid = user.Id;
            this.UserName = user.UserName;
        }

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

        public string UserName
        {
            get;
            set;
        }


        public string PendingPasswordHash { get; set; }
    }
}
