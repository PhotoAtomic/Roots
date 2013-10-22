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
            await uow.RepositoryOf<Domain.User>().AddAsync((Domain.User)newUser);

            ((User)user).Guid = newUser.Id;
            return IdentityResult.Succeeded();
        }

        Task<IdentityResult> IUserStore.DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IUser> IUserStore.FindAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
