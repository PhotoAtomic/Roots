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
    public partial class IdentityStore : IUserClaimStore
    {      


        public async Task<IdentityResult> AddAsync(IUserClaim userClaim, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userClaim.UserId);
            if (user.Claims == null) user.Claims = new List<Domain.Claim>();
            if (user.Claims.Any(x => x.Type == userClaim.ClaimType && x.Value == userClaim.ClaimValue)) return IdentityResult.Failed();
            user.Claims.Add(new Domain.Claim
            {
                Type = userClaim.ClaimType,
                Value = userClaim.ClaimValue,
            });
            return IdentityResult.Succeeded();
        }

        public async Task<IEnumerable<IUserClaim>> GetUserClaimsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            if (user.Claims == null) return Enumerable.Empty<UserClaim>();
            return user.Claims.Select(x => new UserClaim(userId, x));
        }

        async Task<IdentityResult> IUserClaimStore.RemoveAsync(string userId, string claimType, string claimValue, CancellationToken cancellationToken)
        {
            var user = uow.RepositoryOf<Domain.User>().GetById(userId);
            if (user.Claims == null) return IdentityResult.Failed();
            var claimToRemove = user.Claims.Where(x => x.Type == claimType && x.Value == claimValue).SingleOrDefault();
            if (claimToRemove == null) return IdentityResult.Failed();
            user.Claims.Remove(claimToRemove);
            return IdentityResult.Succeeded();
        }
    }
}
