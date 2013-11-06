using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public abstract class Mutator
    {
        private bool alreadyApplied = false;
        internal void Mutate(RepositoryAccessor repositoryAccessor)
        {
            if (alreadyApplied) throw new MutatorAlreadyAppliedException();
            ApplyMutation(repositoryAccessor);
            alreadyApplied = true;
        }

        protected abstract void ApplyMutation(RepositoryAccessor repositoryAccessor);

       
    }
}
