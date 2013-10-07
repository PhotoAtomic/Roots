using Microsoft.AspNet.Identity;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Roots.Identification
{
    class UserManagementStore : Store, IUserManagementStore        
    {

    
        public UserManagementStore(IAsyncUnitOfWork uow):base(uow)
        {
        
        }


        public async Task<IdentityResult> CreateAsync(IUserManagement info, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                var user = await uow.RepositoryOf<User>().GetByIdAsync(info.UserId);                
                
                if (user == null) return IdentityResult.Failed("user management store CreateAsync");
                user.Enabled = !info.DisableSignIn;
                user.LastLogInTimeUtc = info.LastSignInTimeUtc;


                return IdentityResult.Succeeded();
            });
        }

        public IUserManagement CreateNewInstance(string userId)
        {
            return new UserManagement{UserId = userId};
        }

        public async Task<IdentityResult> DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);
                if (user == null) return IdentityResult.Failed("DeleteAsync user management store");


                return IdentityResult.Succeeded();
            });
        }

        public async Task<IUserManagement> FindAsync(string userId, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);
                if (user == null) return null;

                return new UserManagement
                {
                    UserId = userId,
                    DisableSignIn = !user.Enabled,
                    LastSignInTimeUtc = user.LastLogInTimeUtc,
                };
            });
     
        }

        public async Task<IdentityResult> UpdateAsync(IUserManagement info, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                var user = await uow.RepositoryOf<User>().GetByIdAsync(info.UserId);
                if (user == null) return IdentityResult.Failed("UpdateAsync user management store");
                user.Enabled = !info.DisableSignIn;
                user.LastLogInTimeUtc = info.LastSignInTimeUtc;


                return IdentityResult.Succeeded();
            });
            
        }
    }
}
