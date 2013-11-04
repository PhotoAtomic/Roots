using Microsoft.AspNet.Identity;
using Roots.Persistence;
using Roots.Persistence.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Roots.Identification
{
    public partial class UserStore : 
        IUserStore<User>, IUserLoginStore<User>, 
        IUserPasswordStore<User>, IUserClaimStore<User>
    {

        //private readonly IUnitOfWork uow;
        private readonly IUnitOfWorkFactory factory;
              
        public UserStore(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
            //uow = new MemoryCache(factory);
         
        }
        public void Dispose()
        {

        }
    }
}
