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
    public class RoleStore : Store, IRoleStore
    {



        public RoleStore(IAsyncUnitOfWork uow)
            : base(uow)
        {
        }

        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                try
                {
                    var user = await asyncUow.RepositoryOf<User>().GetByIdAsync(userId);
                    if (user == null) throw new ArgumentException(string.Format("user with id {0} not found", userId));
                    user.Roles =
                        (new Domain.Role { Name = roleId })
                        .AsEnumerable()
                        .Union(user.Roles.OrEmpty())
                        .Distinct()
                        .ToList();

                    return IdentityResult.Succeeded();

                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed("AddUserToRoleAsync");
                }
            });
        }

        public async Task<IdentityResult> CreateRoleAsync(IRole role, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                return await Task.FromResult(IdentityResult.Succeeded());
            });
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId, bool failIfNonEmpty, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                var userToClean = asyncUow.RepositoryOf<User>().Where(x => x.Roles.Any(y => y.Name == roleId)).ToListAsync().Result;

                if (!userToClean.IsEmpty() && failIfNonEmpty) return IdentityResult.Failed("Some users have this role");

                foreach (var user in userToClean)
                {
                    var roleToRemove = user.Roles.Where(x => x.Name == roleId).Single();
                    user.Roles.Remove(roleToRemove);
                }

                return await Task.FromResult(IdentityResult.Succeeded());
            });

        }

        public async Task<IRole> FindRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                return await Task.FromResult(new Role { Id = roleId, Name = roleId });
            });
        }

        public async Task<IRole> FindRoleByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                return await Task.FromResult(new Role { Id = roleName, Name = roleName });
            });
        }

        public async Task<IEnumerable<IRole>> GetRolesForUserAsync(string userId, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                var user = await asyncUow.RepositoryOf<User>().GetByIdAsync(userId);
                if (user == null) throw new ArgumentException(string.Format("user with id {0} not found", userId));

                if (user.Roles == null) return Enumerable.Empty<Role>();

                return user.Roles
                    .Select(x => new Role { Id = x.Name, Name = x.Name });                
                    
            });

        }

        public async Task<IEnumerable<string>> GetUsersInRoleAsync(string roleId, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                var users = asyncUow.RepositoryOf<User>().Where(x => x.Roles.Any(y => y.Name == roleId));
                return await Task.FromResult(users.Select(x => ((IUser)x).Id));
            });
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                var user = await asyncUow.RepositoryOf<User>().GetByIdAsync(userId);
                if (user == null) throw new ArgumentException(string.Format("user with id {0} not found", userId));
                if (user.Roles == null) return false;
                return user.Roles.Any(x => x.Name == roleId);
            });

        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                var user = await asyncUow.RepositoryOf<User>().GetByIdAsync(userId);
                if (user == null) throw new ArgumentException(string.Format("user with id {0} not found", userId));
                var removed = user.Roles.Remove(user.Roles.Where(x => x.Name == roleId).SingleOrDefault());
                if (removed) return await Task.FromResult(IdentityResult.Succeeded());
                return await Task.FromResult(IdentityResult.Failed("Role not found for the given user"));
            });

        }

        public async Task<bool> RoleExistsAsync(string roleId, CancellationToken cancellationToken)
        {
            return await Commit(async asyncUow =>
            {
                return await Task.FromResult(asyncUow.RepositoryOf<User>()
                    .Any(u => u.Roles.Any(y => y.Name == roleId)));
            });

        }
    }
}
