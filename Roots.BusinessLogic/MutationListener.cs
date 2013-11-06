using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic
{
    public class MutationListener<TMutator>:MutationEventListener<TMutator> where TMutator : Mutator
    {

        private Action<TMutator> handler;

        public MutationListener(Action<TMutator> handler)
        {
            this.handler = handler;
        }

        protected override void MutationApplied(TMutator t)
        {
            if (handler == null) return;
            handler(t);
        }
    }
}
