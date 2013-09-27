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
        private IAsyncUnitOfWork uow;
                

        public IdentityStore(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
            this.uow = factory.CreateAsyncNew();
            Users = new UserStore(uow);
        }

        public IUserLoginStore Logins
        {
            get;
            private set;
        }

        public IRoleStore Roles
        {
            get { throw new NotImplementedException(); }
        }

        public async Task<IdentityResult> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                await uow.CommitAsync();
                return IdentityResult.Succeeded();
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }
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
            uow.Dispose();
        }
    }
}
