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

        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            if (user.Roles == null) user.Roles = new List<Domain.Role>();
            if (user.Roles.Any(x => x.Name == userId)) return IdentityResult.Failed("role already present");
            user.Roles.Add(new Domain.Role
            {
                Name = roleId,
            });
            return IdentityResult.Succeeded();
        }

        public async Task<IdentityResult> CreateRoleAsync(IRole role, CancellationToken cancellationToken)
        {
            return IdentityResult.Succeeded();
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId, bool failIfNonEmpty, CancellationToken cancellationToken)
        {
            var users = uow.RepositoryOf<Domain.User>().Where(x => x.Roles!=null && x.Roles.Any(y => y.Name == roleId));
            foreach (var user in users)
            {
                foreach (var role in user.Roles.Where(x => x.Name == roleId).ToList())
                {
                    user.Roles.Remove(role);
                }                
            }
            return IdentityResult.Succeeded();
        }

        public async Task<IRole> FindRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            return new Role { Id = roleId, Name = roleId };
        }

        public async Task<IRole> FindRoleByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            return new Role { Id = roleName, Name = roleName };
        }

        public async Task<IEnumerable<IRole>> GetRolesForUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            if (user.Roles == null) user.Roles = new List<Domain.Role>();
            return user.Roles.Select(x => new Role
            {
                Id = x.Name,
                Name = x.Name,
            });
        }

        public async Task<IEnumerable<string>> GetUsersInRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            return uow.RepositoryOf<Domain.User>()
                .Where(x => x.Roles != null && x.Roles.Any(y => y.Name == roleId))
                .Select(x => x.Id.ToString());                
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            if (user == null) return false;
            if (user.Roles == null) return false;
            return user.Roles.Any(x=>x.Name == roleId);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            if (user.Roles == null) return IdentityResult.Failed();
            var roleToRemove = user.Roles.Where(x => x.Name == roleId).SingleOrDefault();
            if (roleToRemove == null) return IdentityResult.Failed();
            user.Roles.Remove(roleToRemove);
            return IdentityResult.Succeeded();
        }

        public async Task<bool> RoleExistsAsync(string roleId, CancellationToken cancellationToken)
        {
            return uow.RepositoryOf<Domain.User>().Any(x => x.Roles != null && x.Roles.Any(y => y.Name == roleId));
        }
    }
}
