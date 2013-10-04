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
    public class UserSecretStore : IUserSecretStore
    {

        private IAsyncUnitOfWork uow;


        public UserSecretStore(IAsyncUnitOfWork uow)
        {
            // TODO: Complete member initialization
            this.uow = uow;
        }

        public async Task<IdentityResult> CreateAsync(IUserSecret userSecret, CancellationToken cancellationToken)
        {
            try
            {

                var user = await
                    uow.RepositoryOf<User>()
                    .Where(x => x.UserName == userSecret.UserName)
                    .SingleOrDefaultAsync();

                if (user == null) return IdentityResult.Failed("No user found");
                if (user.Secret != null) return IdentityResult.Failed("User already have a secret");

                user.Secret = userSecret.Secret;

                return IdentityResult.Succeeded();

            }
            catch (Exception ex)
            {
                return IdentityResult.Failed("CreateAsync user secret store");
            }
        }

        public IUserSecret CreateNewInstance(string userName, string secret)
        {
            return new UserSecret() { Secret = secret, UserName = userName };
        }

        public async Task<IdentityResult> DeleteAsync(string userName, CancellationToken cancellationToken)
        {
            try
            {
                var user = await
                        uow.RepositoryOf<User>()
                        .Where(x => x.UserName == userName)
                        .SingleOrDefaultAsync();

                if (user == null) return IdentityResult.Failed("No user found");

                user.Secret = null;


                return IdentityResult.Succeeded();
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed("DeleteAsync user secret store");
            }

        }

        public async Task<IUserSecret> FindAsync(string userName, CancellationToken cancellationToken)
        {
            try
            {
            
                    var user = await
                            uow.RepositoryOf<User>()
                            .Where(x => x.UserName == userName)
                            .SingleOrDefaultAsync();

                    if (user == null) return null;


                    return new UserSecret() { UserName = userName, Secret = user.Secret };
            
            }
            catch
            {
                return null;
            }
        }

        public async Task<IdentityResult> UpdateAsync(string userName, string newSecret, CancellationToken cancellationToken)
        {
            try
            {
                
                    var user = await
                            uow.RepositoryOf<User>()
                            .Where(x => x.UserName == userName)
                            .SingleOrDefaultAsync();

                    if (user == null) return null;

                    user.Secret = newSecret;

                    return IdentityResult.Succeeded();
                
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed("UpdateAsync user secret store");
            }
        }

        public async Task<bool> ValidateAsync(string userName, string loginSecret, CancellationToken cancellationToken)
        {
            
                var user = await
                        uow.RepositoryOf<User>()
                        .Where(x => x.UserName == userName)
                        .SingleOrDefaultAsync();

                if (user == null || user.Secret == null) return false;

                return user.Secret == loginSecret;
            
        }
    }
}
