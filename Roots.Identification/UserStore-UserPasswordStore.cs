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
    public partial class UserStore : IUserPasswordStore<User>
    {


        async Task<string> IUserPasswordStore<User>.GetPasswordHashAsync(User user)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);                                
                return foundUser.Secret;                
            }
        }

        async Task<bool> IUserPasswordStore<User>.HasPasswordAsync(User user)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);                
                return foundUser.Secret != null;
            }
        }

        async Task IUserPasswordStore<User>.SetPasswordHashAsync(User user, string passwordHash)
        {
            //using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            //{
            //    var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
            //    if (foundUser == null)
            //    {
            //        var newUser = new Domain.User
            //        {
            //            UserName = user.UserName,
            //            Id = user.Guid,
            //            Secret = passwordHash,
            //        };
            //        uow.RepositoryOf<Domain.User>().Add(newUser);
            //    }
            //    else
            //    {

            //        foundUser.Secret = passwordHash;
            //    }
            //    uow.Commit();                                
            //}
            user.PendingPasswordHash = passwordHash;
        }
    }
}
