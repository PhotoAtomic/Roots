using Microsoft.AspNet.Identity;
using Roots.Domain;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoAtomic.Extensions;

namespace Roots.Identification
{
    public partial class IdentityStore : IUserLoginStore
    {

        private class LoginEqualityComparer : IEqualityComparer<Domain.Login>
        {

            public bool Equals(Login x, Login y)
            {
                if (x == null && y == null) return true;
                if (x == null ^ y == null) return false;
                return x.Provider == y.Provider;
            }

            public int GetHashCode(Login obj)
            {
                return obj.Provider.GetHashCode();
            }
        }

        async Task<IdentityResult> IUserLoginStore.AddAsync(IUserLogin login, System.Threading.CancellationToken cancellationToken)
        {
            var user = await GetUserById(login.UserId);
            if (user == null) return IdentityResult.Failed("User not exists");
            if (user.Logins == null) user.Logins = new List<Domain.Login>();
        
            user.Logins = user.Logins.AddIfNotPresent(
                new Domain.Login
                {
                    Provider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                },
                new LoginEqualityComparer())
                .ToList();

            return IdentityResult.Succeeded();

        }

        IUserLogin IUserLoginStore.CreateNewInstance(string userId, string loginProvider, string providerKey)
        {
            return new UserLogin
            {                
                UserId =  userId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey,
            };
        }

        async Task<IEnumerable<IUserLogin>> IUserLoginStore.GetLoginsAsync(string userId, System.Threading.CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return null;
            if (user.Logins == null) return Enumerable.Empty<UserLogin>();
            return user.Logins.Select(x => new UserLogin(userId, x));
        }

        async Task<string> IUserLoginStore.GetProviderKeyAsync(string userId, string loginProvider, System.Threading.CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return null;
            if (user.Logins == null) return null;
            return user.Logins.Where(x => x.Provider == loginProvider).SingleOrDefault().ProviderKey;
        }

        async Task<string> IUserLoginStore.GetUserIdAsync(string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            using (var uow = factory.CreateAsyncNew())
            {
                var user = await uow.RepositoryOf<Domain.User>()
                    .Where(x => x.Logins.Any(y => y.Provider == providerKey && y.ProviderKey == providerKey))
                    .SingleOrDefaultAsync();
                if (user == null) return null;
                var dummyUser = new User() { Guid = user.Id };
                await GetUserById(user.Id);
                return dummyUser.Id;
                    
            }

        }

        async Task<IdentityResult> IUserLoginStore.RemoveAsync(string userId, string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return IdentityResult.Failed("User not exists");
            if (user.Logins == null) user.Logins = new List<Domain.Login>();

            var loginToRemove = new Domain.Login
            {
                Provider = loginProvider,
                ProviderKey = providerKey,
            };

            user.Logins = user.Logins
                .Except(loginToRemove.AsEnumerable(), new LoginEqualityComparer())
                .ToList();            

            return IdentityResult.Succeeded();
        }
    }
}
