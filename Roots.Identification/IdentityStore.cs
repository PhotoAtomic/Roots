using Microsoft.AspNet.Identity;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class IdentityStore : IIdentityStore
    {
        private readonly IUnitOfWorkFactory factory;


        public IdentityStore(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
            Users = new UserStore(factory);
        }

        public IUserLoginStore Logins
        {
            get { throw new NotImplementedException(); }
        }

        public IRoleStore Roles
        {
            get { throw new NotImplementedException(); }
        }

        public Task<IdentityResult> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IUserSecretStore Secrets
        {
            get { throw new NotImplementedException(); }
        }

        public ITokenStore Tokens
        {
            get { throw new NotImplementedException(); }
        }

        public IUserClaimStore UserClaims
        {
            get { throw new NotImplementedException(); }
        }

        public IUserManagementStore UserManagement
        {
            get { throw new NotImplementedException(); }
        }

        public IUserStore Users
        {
            get;
            private set;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
