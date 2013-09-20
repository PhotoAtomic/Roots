using Microsoft.AspNet.Identity;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Roots.Identification.RavenDb
{
    class UserStore : IUserStore
    {
        private IUnitOfWorkFactory unitOfWorkFactory { get; set; }

        public UserStore(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<IdentityResult> CreateAsync(IUser user, CancellationToken cancellationToken)
        {
            using (var uow = unitOfWorkFactory.CreateAsyncNew())
            {
                IUser systemUser = new User();
                systemUser.Id = user.Id;
                systemUser.UserName = user.UserName;
                try
                {
                    await uow.RepositoryOf<User>().AddAsync((User)systemUser);
                    await uow.CommitAsync();
                    return IdentityResult.Succeeded();
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed(ex.Message);
                }
            }
        }

        public async Task<IdentityResult> DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            using (var uow = unitOfWorkFactory.CreateAsyncNew())
            {
                try
                {
                    var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);
                    if (user == null) return IdentityResult.Failed("User not found");
                    await uow.RepositoryOf<User>().RemoveAsync(user);
                    await uow.CommitAsync();
                    return IdentityResult.Succeeded();
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed(ex.Message);
                }
            }
        }

        public async Task<IUser> FindAsync(string userId, CancellationToken cancellationToken)
        {
            using (var uow = unitOfWorkFactory.CreateAsyncNew())
            {
                return await uow.RepositoryOf<User>().GetByIdAsync(userId);
            }
        }

        public async Task<IUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            using (var uow = unitOfWorkFactory.CreateAsyncNew())
            {
                return await uow.RepositoryOf<User>().Where(x => x.UserName == userName).SingleOrDefaultAsync();                
            }
        }


    }
}
