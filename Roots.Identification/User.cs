using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class User : Roots.Domain.User, IUser
    {
        string IUser.Id
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


        string IUser.UserName
        {
            get { return UserName; }
            set { UserName = value; }
        }

        public string UserName
        {
            get
            {
                return string.Format("{0} {1}", base.FirstName, base.LastName);
            }
            set
            {

            }
        }

    }
}
