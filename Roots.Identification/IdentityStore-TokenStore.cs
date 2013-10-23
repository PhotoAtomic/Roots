using Microsoft.AspNet.Identity;
using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public partial class IdentityStore : ITokenStore
    {


        async Task<IdentityResult> ITokenStore.AddAsync(IToken token, CancellationToken cancellationToken)
        {

            uow.RepositoryOf<Domain.SecurityToken>().Add(new Domain.SecurityToken
            {
                Id = ((Token)token).Guid,
                UtcExpireDate = token.ValidUntilUtc,
                Value = token.Value,
            });

            return IdentityResult.Succeeded();
        }

        IToken ITokenStore.CreateNewInstance()
        {
            return new Token();
        }

        async Task<IToken> ITokenStore.FindAsync(string id, bool onlyIfValid, CancellationToken cancellationToken)
        {
            var token = uow.RepositoryOf<Domain.SecurityToken>().GetById(id);
            if (token == null) return null;
            if (token.UtcExpireDate > DateTime.UtcNow || !onlyIfValid)
            {
                return new Token
                {
                    Guid = token.Id,
                    UtcExpireDate = token.UtcExpireDate,
                    Value = token.Value,
                };
            }            
            return null;
                

        }

        async Task<IdentityResult> ITokenStore.RemoveAsync(string token, CancellationToken cancellationToken)
        {
            uow.RepositoryOf<Domain.SecurityToken>().RemoveById(token);
            return IdentityResult.Succeeded();
        }

        async Task<IdentityResult> ITokenStore.UpdateAsync(IToken token, CancellationToken cancellationToken)
        {
            var tokenToUpdate = uow.RepositoryOf<Domain.SecurityToken>().GetById(token.Id);
            tokenToUpdate.UtcExpireDate = token.ValidUntilUtc;
            tokenToUpdate.Value = token.Value;

            return IdentityResult.Succeeded();

        }
    }
}
