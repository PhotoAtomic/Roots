using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public abstract class MutationEventListener
        <T> : IMutationEventListener where T : Mutator
    {

        public void OnMutation(Mutator mutator)
        {
            MutationApplied((T)mutator);
        }

        protected abstract void MutationApplied(T t);
    }
}
