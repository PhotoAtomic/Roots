using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public abstract class Selector<T>
    {
        protected internal T Select(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            return PerformSelection(repositoryAccessor);
        }

        protected abstract T PerformSelection(ReadOnlyRepositoryAccessor repositoryAccessor);
    }
}
