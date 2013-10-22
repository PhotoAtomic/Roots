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



        public Task<IdentityResult> AddAsync(IUserClaim userClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IUserClaim>> GetUserClaimsAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserClaimStore.RemoveAsync(string userId, string claimType, string claimValue, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
