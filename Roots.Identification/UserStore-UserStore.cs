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
    public partial class UserStore : IUserStore<User>
    {

        

        async Task IUserStore<User>.CreateAsync(User user)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var newUser = new Domain.User
                {
                    UserName = user.UserName,
                    Id = Guid.Parse(user.Id),
                    Secret = user.PendingPasswordHash,
                };
                uow.RepositoryOf<Domain.User>().Add((Domain.User)newUser);

                ((User)user).Guid = newUser.Id;
                uow.Commit();
            }
        }

        async Task IUserStore<User>.DeleteAsync(User user)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                uow.RepositoryOf<Domain.User>().RemoveById(user.Id);
                uow.Commit();
            }
        }

        async Task<User> IUserStore<User>.FindByIdAsync(string userId)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var user = uow.RepositoryOf<Domain.User>().GetById(userId);
                if (user == null) return null;
                return new User
                {
                    Guid = user.Id,
                    UserName = user.UserName,
                };
                uow.Commit();
            }
        }

        async Task<User> IUserStore<User>.FindByNameAsync(string userName)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userName).SingleOrDefault();
                if (user == null) return null;
                return new User
                {
                    Guid = user.Id,
                    UserName = user.UserName,
                };
                uow.Commit();
            }
        }

        async Task IUserStore<User>.UpdateAsync(User user)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
                foundUser.UserName = user.UserName;
                if (user.PendingPasswordHash != null)
                {
                    foundUser.Secret = user.PendingPasswordHash;
                }
                uow.Commit();
            }
        }
    }
}
