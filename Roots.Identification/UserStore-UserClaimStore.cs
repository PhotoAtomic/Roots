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
    public partial class UserStore : IUserClaimStore<User>
    {      

        async Task IUserClaimStore<User>.AddClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
                if (foundUser.Claims == null) foundUser.Claims = new List<Domain.Claim>();
                if (foundUser.Claims.Any(x => x.Type == claim.Type && x.Value == claim.Value)) return;
                foundUser.Claims.Add(new Domain.Claim
                {
                    Type = claim.Type,
                    Value = claim.Value,
                });
                uow.Commit();
            }
        }

        async Task<IList<System.Security.Claims.Claim>> IUserClaimStore<User>.GetClaimsAsync(User user)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
                if (foundUser.Claims == null) return Enumerable.Empty<System.Security.Claims.Claim>().ToList();
                return foundUser.Claims
                    .Select(x => new System.Security.Claims.Claim(x.Type, x.Value))
                    .ToList();
            }
        }

        async Task IUserClaimStore<User>.RemoveClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var foundUser = uow.RepositoryOf<Domain.User>().GetById(user.Id);
                if (foundUser.Claims == null) foundUser.Claims = new List<Domain.Claim>();
                var claimsToRemove = foundUser.Claims.Where(x => x.Type == claim.Type && x.Value == claim.Value);
                foreach (var claimToRemove in claimsToRemove)
                {
                    foundUser.Claims.Remove(claimToRemove);
                }
                uow.Commit();
            }
        }
        
    }
}
