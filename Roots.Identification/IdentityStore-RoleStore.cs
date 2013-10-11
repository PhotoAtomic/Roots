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

        async Task<IdentityResult> IRoleStore.AddUserToRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return IdentityResult.Failed("user not found");
            if (user.Roles == null) user.Roles = new List<Domain.Role>();

            user.Roles = user.Roles.AddIfNotPresent(
                new Domain.Role
                {
                    Name = roleId
                },
                (x, y) => x.Name == y.Name,
                x => (x.Name == null) ? 0 : x.Name.GetHashCode())
                .ToList();

            return IdentityResult.Succeeded();
        }

        async Task<IdentityResult> IRoleStore.CreateRoleAsync(IRole role, CancellationToken cancellationToken)
        {
            return IdentityResult.Succeeded();
        }

        async Task<IdentityResult> IRoleStore.DeleteRoleAsync(string roleId, bool failIfNonEmpty, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

        }

        async Task<IRole> IRoleStore.FindRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<IRole> IRoleStore.FindRoleByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<IRole>> IRoleStore.GetRolesForUserAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<string>> IRoleStore.GetUsersInRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IRoleStore.IsUserInRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

        }

        async Task<IdentityResult> IRoleStore.RemoveUserFromRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IRoleStore.RoleExistsAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
