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
    public partial class IdentityStore : IUserSecretStore
    {

        public Task<IdentityResult> CreateAsync(IUserSecret userSecret, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userSecret.UserName).Single();
            user.Secret = userSecret.Secret;
            return Task.FromResult(IdentityResult.Succeeded());
        }

        public IUserSecret CreateNewInstance(string userName, string secret)
        {
            return new UserSecret
            {
                Secret = secret,
                UserName = userName,
            };
        }

        async Task<IdentityResult> IUserSecretStore.DeleteAsync(string userName, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userName).Single();
            user.Secret = null;
            return IdentityResult.Succeeded();

        }

        async Task<IUserSecret> IUserSecretStore.FindAsync(string userName, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userName).Single();

            return new UserSecret
            {
                Secret = user.Secret,
                UserName = user.UserName,
            };
        }

        public async Task<IdentityResult> UpdateAsync(string userName, string newSecret, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userName).Single();
            user.Secret = newSecret;
            return IdentityResult.Succeeded();

        }

        public async Task<bool> ValidateAsync(string userName, string loginSecret, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userName).Single();
            return user.Secret == loginSecret;
        }
    }
}
