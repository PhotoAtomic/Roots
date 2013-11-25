using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic
{
    public abstract class Extractor<T>
    {
        protected internal T Extract(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            return PerformExtraction(repositoryAccessor);
        }

        protected abstract T PerformExtraction(ReadOnlyRepositoryAccessor repositoryAccessor);
    }
}
