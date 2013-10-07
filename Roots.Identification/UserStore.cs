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
    public class UserStore : Store, IUserStore
    {
        
        public UserStore(IAsyncUnitOfWork uow):base(uow)
        {            
        }

        public async Task<IdentityResult> CreateAsync(IUser user, CancellationToken cancellationToken)
        {

            return await Commit(async uow =>
            {
                IUser systemUser = new User();
                systemUser.Id = user.Id;
                systemUser.UserName = user.UserName;
                try
                {

                    await uow.RepositoryOf<User>().AddAsync((User)systemUser);

                    user.Id = systemUser.Id;
                    return IdentityResult.Succeeded();

                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed("CreateAsync user store");
                }
            });

        }

        public async Task<IdentityResult> DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                try
                {
                    var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);
                    if (user == null) return IdentityResult.Failed("User not found");
                    await uow.RepositoryOf<User>().RemoveAsync(user);

                    return IdentityResult.Succeeded();

                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed("DeleteAsync user store");
                }
            });

        }

        public async Task<IUser> FindAsync(string userId, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                return await uow.RepositoryOf<User>().GetByIdAsync(userId);
            });

        }

        public async Task<IUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                var repo = uow.RepositoryOf<User>();
                return await repo.Where(x => x.UserName == userName).SingleOrDefaultAsync();
            });
        }

    }
}
