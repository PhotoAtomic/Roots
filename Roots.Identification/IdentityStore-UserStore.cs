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

        public async Task<IdentityResult> CreateAsync(IUser user, CancellationToken cancellationToken)
        {
            var newUser = new Domain.User{
                UserName = user.UserName,
                Id = Guid.Parse(user.Id),
            };
            uow.RepositoryOf<Domain.User>().Add((Domain.User)newUser);

            ((User)user).Guid = newUser.Id;
            return IdentityResult.Succeeded();
        }

        Task<IdentityResult> IUserStore.DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<IUser> IUserStore.FindAsync(string userId, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            return new User
            {
                Guid = user.Id,
                UserName = user.UserName,
            };
        }

        public async Task<IUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userName).SingleOrDefault();
            return new User
            {
                Guid = user.Id,
                UserName = user.UserName,
            };
        }
    }
}
