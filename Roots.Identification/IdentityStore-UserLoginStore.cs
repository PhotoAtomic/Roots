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

        public async Task<IdentityResult> AddAsync(IUserLogin login, System.Threading.CancellationToken cancellationToken)
        {
            var loginEqualityComparer = new ItemEqualityComparer<Domain.Login>(
                (a,b)=>
                    string.Equals(a.Provider,b.Provider) && string.Equals(a.ProviderKey,b.ProviderKey),
                l=>{
                        unchecked
                        {
                            return ((l.Provider==null)?0:l.Provider.GetHashCode()) + ((l.ProviderKey==null)?0:l.ProviderKey.GetHashCode());
                        }
                   }
                );

            

            var user = uow.RepositoryOf<Domain.User>().GetById(login.UserId);
            var loginToStore = new Domain.Login
            {
                Provider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };
            if (user.Logins == null) user.Logins = new List<Domain.Login>();
            if (!user.Logins.Contains(loginToStore, loginEqualityComparer))
            {
                user.Logins.Add(loginToStore);
                return IdentityResult.Succeeded();
            }
            return IdentityResult.Failed();
        }

        public IUserLogin CreateNewInstance(string userId, string loginProvider, string providerKey)
        {
            return new UserLogin
            {
                LoginProvider = loginProvider,
                UserId = userId,
                ProviderKey = providerKey,
            };
        }

        public async Task<IEnumerable<IUserLogin>> GetLoginsAsync(string userId, System.Threading.CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            return user.Logins.Select(x=>new UserLogin(userId,x));
        }

        public async Task<string> GetProviderKeyAsync(string userId, string loginProvider, System.Threading.CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            return user.Logins.Where(x => x.Provider == loginProvider).Select(x => x.ProviderKey).SingleOrDefault();
        }

        public async Task<string> GetUserIdAsync(string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.Logins != null && x.Logins.Any(y => y.ProviderKey == providerKey && y.Provider == loginProvider)).FirstOrDefault();

            

            //var login = uow.RepositoryOf<Domain.Login>().Where(x => x.Provider == loginProvider && x.ProviderKey == providerKey).SingleOrDefault();
            //if (login == null) return null;
            //var user = uow.RepositoryOf<Domain.User>().Where(x => x.Logins!=null && x.Logins.Contains(login)).SingleOrDefault();
            return user.Id.ToString();
        }

        async Task<IdentityResult> IUserLoginStore.RemoveAsync(string userId, string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            var loginToRemove = user.Logins.Where(x => x.ProviderKey == providerKey && x.Provider == loginProvider).SingleOrDefault();
            if (loginToRemove == null) return IdentityResult.Failed();
            user.Logins.Remove(loginToRemove);
            return IdentityResult.Succeeded();
        }
    }
}
