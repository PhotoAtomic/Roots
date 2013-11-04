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
    public partial class UserStore : IUserLoginStore<User>
    {

    

        async Task IUserLoginStore<User>.AddLoginAsync(User user, UserLoginInfo login)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {

                var loginEqualityComparer = new ItemEqualityComparer<Domain.Login>(
                    (a, b) =>
                        string.Equals(a.Provider, b.Provider) && string.Equals(a.ProviderKey, b.ProviderKey),
                    (l) =>
                    {
                        unchecked
                        {
                            return ((l.Provider == null) ? 0 : l.Provider.GetHashCode()) + ((l.ProviderKey == null) ? 0 : l.ProviderKey.GetHashCode());
                        }
                    });


                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
                var loginToStore = new Domain.Login
                {
                    Provider = login.LoginProvider,
                    ProviderKey = login.ProviderKey
                };
                if (foundUser.Logins == null) foundUser.Logins = new List<Domain.Login>();
                if (!foundUser.Logins.Contains(loginToStore, loginEqualityComparer))
                {
                    foundUser.Logins.Add(loginToStore);
                    uow.Commit();
                }               
            }
        }

        async Task<User> IUserLoginStore<User>.FindAsync(UserLoginInfo login)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().Where(x => x.Logins != null && x.Logins.Any(y => y.ProviderKey == login.ProviderKey && y.Provider == login.LoginProvider)).FirstOrDefault();
                return new User(foundUser);
            }
        }

        async Task<IList<UserLoginInfo>> IUserLoginStore<User>.GetLoginsAsync(User user)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
                return foundUser.Logins.Select(x => new UserLoginInfo(x.Provider,x.ProviderKey)).ToList();
            }
        }

        async Task IUserLoginStore<User>.RemoveLoginAsync(User user, UserLoginInfo login)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
                var loginToRemove = foundUser.Logins.Where(x => x.ProviderKey == login.ProviderKey && x.Provider == login.LoginProvider).SingleOrDefault();
                foundUser.Logins.Remove(loginToRemove);
                uow.Commit();
            }
        }
    }
}
