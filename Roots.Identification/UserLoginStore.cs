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
                return IdentityResult.Failed(ex.Message);
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
                .Select(x=>x.ProviderKey)
                .SingleOrDefault();
        }

        public Task<string> GetUserIdAsync(string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            var userId = uow.RepositoryOf<User>()
                .Where(x => x.Logins.Any(y => y.Provider == loginProvider && y.ProviderKey == providerKey))
                .Select(x=>((IUser)x).Id)
                .SingleOrDefault();
            return Task.FromResult(userId);
        }

        public async Task<IdentityResult> RemoveAsync(string userId, string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);
                await uow.RepositoryOf<User>().RemoveAsync(user);
                return IdentityResult.Succeeded();
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }

        }
    }
}
