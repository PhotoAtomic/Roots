using Microsoft.AspNet.Identity;
using Roots.Domain;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoAtomic.Extensions;

namespace Roots.Identification
{
    public partial class IdentityStore : IUserLoginStore
    {

        public Task<IdentityResult> AddAsync(IUserLogin login, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IUserLogin CreateNewInstance(string userId, string loginProvider, string providerKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IUserLogin>> GetLoginsAsync(string userId, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetProviderKeyAsync(string userId, string loginProvider, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserLoginStore.RemoveAsync(string userId, string loginProvider, string providerKey, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
