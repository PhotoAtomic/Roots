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
    public partial class IdentityStore : IUserStore
    {

        private IList<Domain.User> usersRepository = new List<Domain.User>();

        private IList<Guid> usersToRemove = new List<Guid>();

        private async Task<Domain.User> GetUser(Func<IQueryable<Domain.User>,Domain.User> by, bool createIfNotFound = false)
        {
            Domain.User user;        

            lock(usersRepository)
            {                                
                user =  by(usersRepository.AsQueryable());
                if (user != null) return user;
            }

            using (var uow = factory.CreateAsyncNew())
            {                
                user = by(uow.RepositoryOf<Domain.User>());
            }            
            if (user == null)
            {
                if (!createIfNotFound) return null;
                user = new Domain.User { Id = Guid.NewGuid() };
            }

            lock(usersRepository)
            {
                Domain.User alreadyPresentUser;

                alreadyPresentUser = by(usersRepository.AsQueryable());
                if (alreadyPresentUser != null) return alreadyPresentUser;


                usersRepository.Add(user);
                return user;
            }
        }

        private async Task<Domain.User> GetUserById(string userId, bool createIfNotFound = false)
        {
            User dummyUser = new User { Id = userId };
            var user = await GetUser(
                r => r.Where(u => u.Id == dummyUser.Guid).SingleOrDefaultAsync().Result,
                createIfNotFound);
            return user;
        }

        private async Task<Domain.User> GetUserById(Guid id, bool createIfNotFound = false)
        {
            var user = await GetUser(
                r => r.Where(u => u.Id == id).SingleOrDefaultAsync().Result,
                createIfNotFound);
            return user;
        }

        async Task<Domain.User> GetUserByName(string userName, bool createIfNotFound = false)
        {
            var user = await GetUser(
                r => r.Where(u => u.UserName == userName).SingleOrDefaultAsync().Result,
                createIfNotFound);
            return user;
        }





        async Task<IdentityResult> IUserStore.CreateAsync(IUser user, CancellationToken cancellationToken)
        {            
            var workingUser = await GetUserById(user.Id, true);
            workingUser.UserName = user.UserName;
            return IdentityResult.Succeeded();
        }



        async Task<IdentityResult> IUserStore.DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return IdentityResult.Failed();

            usersRepository.Remove(user);
            usersToRemove.Add(user.Id);

            return IdentityResult.Succeeded();
        }

        async Task<IUser> IUserStore.FindAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return null;
            return new User { Guid = user.Id, UserName = user.UserName };
        }

        async Task<IUser> IUserStore.FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await GetUserByName(userName);
            return new User { Guid = user.Id, UserName = user.UserName };
        }

        
    }
}
