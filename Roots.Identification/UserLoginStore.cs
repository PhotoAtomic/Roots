using Microsoft.AspNet.Identity;
using Roots.Domain;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class UserLoginStore : IUserLoginStore
    {


        private IAsyncUnitOfWork uow;
     

        public UserLoginStore(IAsyncUnitOfWork uow)
        {
            // TODO: Complete member initialization
            this.uow = uow;
        }


        public async Task<IdentityResult> AddAsync(IUserLogin login, System.Threading.CancellationToken cancellationToken)
        {
            try
            {

                var user = await uow.RepositoryOf<User>().GetByIdAsync(login.UserId);
                if (user.Logins == null) user.Logins = new List<Login>();
                user.Logins.Add(new Login
                    {
                        Provider = login.LoginProvider,
                        ProviderKey = login.ProviderKey,
                    });

                return IdentityResult.Succeeded();

            }
            catch (Exception ex)
            {
                return IdentityResult.Failed("AddAsync user login store");
            }

        }

        public IUserLogin CreateNewInstance(string userId, string loginProvider, string providerKey)
        {
            return new UserLogin
            {
                UserId = userId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey,
            };
        }

        public async Task<IEnumerable<IUserLogin>> GetLoginsAsync(string userId, System.Threading.CancellationToken cancellationToken)
        {

            var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);

            return user.Logins.Select(x => new UserLogin
                {
                    UserId = ((IUser)user).Id,
                    LoginProvider = x.Provider,
                    ProviderKey = x.ProviderKey,
                });

        }

        public async Task<string> GetProviderKeyAsync(string userId, string loginProvider, System.Threading.CancellationToken cancellationToken)
        {

            var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);

            return user
                .Logins
                .Where(x => x.Provider == loginProvider)
                .Select(x => x.ProviderKey)
                .SingleOrDefault();

        }

        public async Task<string> GetUserIdAsync(string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {

            var user = await uow.RepositoryOf<User>()
                .Where(x => x.Logins.Any(y => y.Provider == loginProvider && y.ProviderKey == providerKey))
                .SingleOrDefaultAsync();

            return ((IUser)user).Id;

        }

        public async Task<IdentityResult> RemoveAsync(string userId, string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            try
            {

                var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);
                if (user == null) return IdentityResult.Failed("userid not found");

                var loginToRemove = user.Logins
                    .Where(x => x.Provider == loginProvider && x.ProviderKey == providerKey)
                    .SingleOrDefault();

                if (loginToRemove == null) return IdentityResult.Failed("pair (loginProvider, providerKey) not found for the given user");

                if (!user.Logins.Remove(loginToRemove)) return IdentityResult.Failed("for some reason it is not possible to remove the login from the given user");


                return IdentityResult.Succeeded();

            }
            catch (Exception ex)
            {
                return IdentityResult.Failed("remove async user login store");
            }

        }
    }
}
