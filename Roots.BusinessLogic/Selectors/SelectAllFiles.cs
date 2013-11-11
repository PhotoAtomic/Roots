using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Selectors
{
    public class SelectAllFiles : Selector<IEnumerable<string>>
    {
        protected override IEnumerable<string> PerformSelection(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            return repositoryAccessor.RepositoryOf<Domain.FileContent>().Select(x => x.SorceName).ToList();
        }
    }
}
