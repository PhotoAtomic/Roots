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
    class TokenStore : Store, ITokenStore
    {

        public TokenStore(IAsyncUnitOfWork uow)
            : base(uow)
        { }

        public async Task<IdentityResult> AddAsync(IToken token, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {

                try
                {

                    IToken newToken = new Token();
                    newToken.Id = token.Id;
                    newToken.ValidUntilUtc = token.ValidUntilUtc;
                    newToken.Value = token.Value;

                    await uow.RepositoryOf<Token>().AddAsync((Token)newToken);


                    return IdentityResult.Succeeded();

                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed("AddAsync token store");
                }
            });
        }

        public IToken CreateNewInstance()
        {

            return new Token()
            {
                Id = Guid.NewGuid(),
                UtcExpireDate = DateTime.UtcNow + new TimeSpan(5, 0, 0, 0), // 5 days                
            };

        }

        public async Task<IToken> FindAsync(string id, bool onlyIfValid, CancellationToken cancellationToken)
        {

            return await Commit(async uow =>
            {
                var token = await uow.RepositoryOf<Token>().GetByIdAsync(id);
                if (token == null) return null;
                if (onlyIfValid && token.UtcExpireDate < DateTime.UtcNow) return token;
                return token;
            });

        }

        public async Task<IdentityResult> RemoveAsync(string token, CancellationToken cancellationToken)
        {
            return await Commit(async uow =>
            {
                var foundToken = await uow.RepositoryOf<Token>().GetByIdAsync(token);
                if (foundToken == null) return IdentityResult.Failed("Token not found");
                await uow.RepositoryOf<Token>().RemoveAsync(foundToken);

                return IdentityResult.Succeeded();
            });

        }

        public async Task<IdentityResult> UpdateAsync(IToken token, CancellationToken cancellationToken)
        {

            return await Commit(async uow =>
            {
                IToken foundToken = await uow.RepositoryOf<Token>().GetByIdAsync(token.Id);
                if (foundToken == null) return IdentityResult.Failed("Token not found");

                foundToken.ValidUntilUtc = token.ValidUntilUtc;
                foundToken.Value = token.Value;

                return IdentityResult.Succeeded();
            });
        }
    }
}
