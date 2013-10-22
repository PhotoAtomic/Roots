using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PhotoAtomic.Extensions;
using Roots.Persistence;

namespace Roots.Identification
{
    public partial class IdentityStore : IRoleStore
    {

        public Task<IdentityResult> AddUserToRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateRoleAsync(IRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteRoleAsync(string roleId, bool failIfNonEmpty, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> FindRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> FindRoleByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRole>> GetRolesForUserAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetUsersInRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleExistsAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
