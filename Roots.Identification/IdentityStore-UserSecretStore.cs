using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Roots.Persistence;

namespace Roots.Identification
{
    public partial class IdentityStore : IUserSecretStore
    {


        async Task<IdentityResult> IUserSecretStore.CreateAsync(IUserSecret userSecret, CancellationToken cancellationToken)
        {
            var user = await GetUserByName(userSecret.UserName);
            if(user == null)IdentityResult.Failed("user not found");
            if (user.Secret != null) IdentityResult.Failed("user already have a password");
            user.Secret = userSecret.Secret;
            return IdentityResult.Succeeded();
        }

        IUserSecret IUserSecretStore.CreateNewInstance(string userName, string secret)
        {
            return new UserSecret { UserName = userName, Secret = secret };
        }

        async Task<IdentityResult> IUserSecretStore.DeleteAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await GetUserByName(userName);
            if (user == null) IdentityResult.Failed("user not found");
            user.Secret = null;
            return IdentityResult.Succeeded();
        }

        async Task<IUserSecret> IUserSecretStore.FindAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await GetUserByName(userName);
            if (user == null) IdentityResult.Failed("user not found");
            return ((IUserSecretStore)this).CreateNewInstance(user.UserName, user.Secret);
        }

        async Task<IdentityResult> IUserSecretStore.UpdateAsync(string userName, string newSecret, CancellationToken cancellationToken)
        {
            var user = await GetUserByName(userName);
            if (user == null) IdentityResult.Failed("user not found");
            user.Secret = newSecret;
            return IdentityResult.Succeeded();
         
        }

        async Task<bool> IUserSecretStore.ValidateAsync(string userName, string loginSecret, CancellationToken cancellationToken)
        {
            var user = await GetUserByName(userName);
            if (user == null) return false;
            return user.Secret == loginSecret;            
        }
    }
}
