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

        async Task<IdentityResult> IUserManagementStore.CreateAsync(IUserManagement info, CancellationToken cancellationToken)
        {
            var user = await GetUserById(info.UserId);
            if (user == null) return IdentityResult.Failed("user not found");
            user.Enabled = !info.DisableSignIn;
            user.LastLogInTimeUtc = info.LastSignInTimeUtc;
            return IdentityResult.Succeeded();
        }

        IUserManagement IUserManagementStore.CreateNewInstance(string userId)
        {
            return new UserManagement { UserId = userId };
        }

        async Task<IdentityResult> IUserManagementStore.DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            return IdentityResult.Succeeded();
        }

        async Task<IUserManagement> IUserManagementStore.FindAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return null;
            
            var dummyUser = new User{ Guid = user.Id};

            return new UserManagement
            {
                DisableSignIn = !user.Enabled,
                LastSignInTimeUtc = user.LastLogInTimeUtc,
                UserId = dummyUser.Id,
            };
                            
        }

        async Task<IdentityResult> IUserManagementStore.UpdateAsync(IUserManagement info, CancellationToken cancellationToken)
        {
            var user = await GetUserById(info.UserId);
            if (user == null) return IdentityResult.Failed("user not found");
            user.Enabled = !info.DisableSignIn;
            user.LastLogInTimeUtc = info.LastSignInTimeUtc;
            return IdentityResult.Succeeded();            
        }
    }
}
