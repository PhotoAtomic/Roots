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

        public async Task<IdentityResult> CreateAsync(IUserSecret userSecret, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().Where(x => x.UserName == userSecret.UserName).Single();
            user.Secret = userSecret.Secret;
            return IdentityResult.Succeeded();
        }

        public IUserSecret CreateNewInstance(string userName, string secret)
        {
            return new UserSecret
            {
                Secret = secret,
                UserName = userName,
            };
        }

        Task<IdentityResult> IUserSecretStore.DeleteAsync(string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IUserSecret> IUserSecretStore.FindAsync(string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(string userName, string newSecret, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateAsync(string userName, string loginSecret, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
