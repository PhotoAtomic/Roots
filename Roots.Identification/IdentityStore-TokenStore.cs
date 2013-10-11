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
            throw new NotImplementedException();
        }

        IToken ITokenStore.CreateNewInstance()
        {
            throw new NotImplementedException();
           
        }

        async Task<IToken> ITokenStore.FindAsync(string id, bool onlyIfValid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
           

        }

        async Task<IdentityResult> ITokenStore.RemoveAsync(string token, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task<IdentityResult> ITokenStore.UpdateAsync(IToken token, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
           
        }
    }
}
