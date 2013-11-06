using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public interface IMutationEventListener
    {        
        void OnMutation(Mutator mutator);
    }
}
