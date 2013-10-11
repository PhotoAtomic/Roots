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

        private class ClaimEqualityComparer : IEqualityComparer<Domain.Claim>
        {

            public bool Equals(Domain.Claim x, Domain.Claim y)
            {
                if (x == null && y == null) return true;
                if (x == null ^ y == null) return false;
                return (x.Type == y.Type) && (x.Value == y.Value);
            }

            public int GetHashCode(Domain.Claim obj)
            {
                unchecked
                {
                    return obj.Value.GetHashCode() + obj.Type.GetHashCode();
                }
            }
        }


        async Task<IdentityResult> IUserClaimStore.AddAsync(IUserClaim userClaim, CancellationToken cancellationToken)
        {
            var user = await GetUserById(userClaim.UserId);
            if (user == null) return IdentityResult.Failed("User not exists");
            if (user.Claims == null) user.Claims = new List<Domain.Claim>();
            user.Claims = user.Claims.AddIfNotPresent(
                new Domain.Claim
                {
                    Type = userClaim.ClaimType,
                    Value = userClaim.ClaimValue,
                },
                new ClaimEqualityComparer())
            .ToList();


            return IdentityResult.Succeeded();

        }

        async Task<IEnumerable<IUserClaim>> IUserClaimStore.GetUserClaimsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return null;
            if (user.Claims == null) return Enumerable.Empty<UserClaim>();
            return user.Claims.Select(x => new UserClaim(userId, x));
        }

        async Task<IdentityResult> IUserClaimStore.RemoveAsync(string userId, string claimType, string claimValue, CancellationToken cancellationToken)
        {
            var user = await GetUserById(userId);
            if (user == null) return IdentityResult.Failed("User not exists");
            if (user.Claims == null) user.Claims = new List<Domain.Claim>();

            var claimToRemove = new Domain.Claim
            {
                Type = claimType,
                Value = claimValue,
            };

            user.Claims = user.Claims
                .Except(claimToRemove.AsEnumerable(), new ClaimEqualityComparer())
                .ToList();

            return IdentityResult.Succeeded();
        }
    }
}
