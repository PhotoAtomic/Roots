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
    public class UserClaimStore : IUserClaimStore
    {       
    
        private IAsyncUnitOfWork uow;

       

        public UserClaimStore(IAsyncUnitOfWork uow)
        {
            // TODO: Complete member initialization
            this.uow = uow;
        }

        public async Task<IdentityResult> AddAsync(IUserClaim userClaim, CancellationToken cancellationToken)
        {
            
                var user = await uow.RepositoryOf<User>().GetByIdAsync(userClaim.UserId);
                if (user == null) return IdentityResult.Failed("User not found");
                if (user.Claims == null) user.Claims = new List<Domain.Claim>();
                if (user.Claims.Any(x => x.Type == userClaim.ClaimType && x.Value == userClaim.ClaimValue)) return IdentityResult.Succeeded();
                user.Claims.Add(new Domain.Claim { Type = userClaim.ClaimType, Value = userClaim.ClaimValue });

             
                return IdentityResult.Succeeded();
            
        }

        public async Task<IEnumerable<IUserClaim>> GetUserClaimsAsync(string userId, CancellationToken cancellationToken)
        {
            
                var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);

                if (user == null) return null;
                if (user.Claims == null) return Enumerable.Empty<UserClaim>();

                return user.Claims.Select(x =>
                    new UserClaim
                    {
                        UserId = userId,
                        ClaimType = x.Type,
                        ClaimValue = x.Value
                    });
            
        }

        public async Task<IdentityResult> RemoveAsync(string userId, string claimType, string claimValue, CancellationToken cancellationToken)
        {
            
                var user = await uow.RepositoryOf<User>().GetByIdAsync(userId);

                if (user == null) return IdentityResult.Failed("User not found");
                if (user.Claims == null) return IdentityResult.Failed("Claim not found");

                var claimsToRemove = user.Claims
                    .Where(x => x.Type == claimType && x.Value == claimValue);

                foreach (var claim in claimsToRemove)
                {
                    user.Claims.Remove(claim);
                }

            
                return IdentityResult.Succeeded();
            
        }
    }
}
