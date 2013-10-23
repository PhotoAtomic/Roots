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
    public partial class IdentityStore : IUserManagementStore
    {

        public async Task<IdentityResult> CreateAsync(IUserManagement info, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(info.UserId);
            user.LastLogInTimeUtc = info.LastSignInTimeUtc;
            user.Enabled = !info.DisableSignIn;
            return IdentityResult.Succeeded();
        }

        public IUserManagement CreateNewInstance(string userId)
        {
            return new UserManagement()
            {
                UserId = userId,
            };

        }

        public Task<IdentityResult> DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Succeeded());
        }

        public async Task<IUserManagement> FindAsync(string userId, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            return new UserManagement
            {
                DisableSignIn = !user.Enabled,
                UserId = userId,
                LastSignInTimeUtc = user.LastLogInTimeUtc,
            };            
        }

        public async Task<IdentityResult> UpdateAsync(IUserManagement info, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(info.UserId);
            user.Enabled = !info.DisableSignIn;
            user.LastLogInTimeUtc = info.LastSignInTimeUtc;
            return IdentityResult.Succeeded();
        }
    }
}
