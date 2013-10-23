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
    public partial class IdentityStore : 
        IIdentityStore, IUserStore, IUserLoginStore, IRoleStore, 
        IUserSecretStore, ITokenStore, IUserClaimStore, IUserManagementStore
    {

        private readonly IUnitOfWork uow;


        
        
        public IdentityStore(IUnitOfWorkFactory factory)
        {
            //uow = factory.CreateAsyncNew(Roots.Persistence.IsolationLevel.ReadItsOwnWrite);
            uow = new MemoryCache(factory);

            Users = (IUserStore)this;
            Logins = (IUserLoginStore)this;
            Roles = (IRoleStore)this;
            Secrets = (IUserSecretStore)this;
            Tokens = (ITokenStore)this;
            UserClaims = (IUserClaimStore)this;
            UserManagement = (IUserManagementStore)this;
        }


        public IUserStore Users
        {
            get;
            private set;
        }

        public IUserLoginStore Logins
        {
            get;
            private set;
        }

        public IRoleStore Roles
        {
            get;
            private set;
        }

        public IUserSecretStore Secrets
        {
            get;
            private set;
        }

        public ITokenStore Tokens
        {
            get;
            private set;
        }

        public IUserClaimStore UserClaims
        {
            get;
            private set;
        }

        public IUserManagementStore UserManagement
        {
            get;
            private set;
        }

        public async Task<IdentityResult> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            uow.Commit();
            return await Task.FromResult(IdentityResult.Succeeded());
        }

       


        public void Dispose()
        {

        }
    }
}
