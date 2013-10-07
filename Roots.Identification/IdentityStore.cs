using Microsoft.AspNet.Identity;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Roots.Identification
{
    public class IdentityStore : IIdentityStore
    {
        private readonly IUnitOfWorkFactory factory;
        private IAsyncUnitOfWork uow;
        private TransactionScope transaction;
                

        public IdentityStore(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
            transaction = new TransactionScope();
            this.uow = factory.CreateAsyncNew(Roots.Persistence.IsolationLevel.ReadItsOwnWrite);
            Users = new UserStore(uow);
            Logins = new UserLoginStore(uow);
            Roles = new RoleStore(uow);
            Secrets = new UserSecretStore(uow);
            Tokens = new TokenStore(uow);
            UserClaims = new UserClaimStore(uow);
            UserManagement = new UserManagementStore(uow);
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
            try
            {
                await uow.CommitAsync();
                transaction.Complete();
                //transaction.Dispose();
                transaction = new TransactionScope();
                return IdentityResult.Succeeded();
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }
        }

       


        public void Dispose()
        {
            uow.Dispose();
            //transaction.Dispose();
        }
    }
}
