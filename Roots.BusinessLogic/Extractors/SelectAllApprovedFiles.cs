using Roots.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Extractors
{
    public class SelectAllApprovedFiles : Selector<IEnumerable<FileContent>>
    {
        protected override IEnumerable<FileContent> PerformExtraction(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            return repositoryAccessor.RepositoryOf<Domain.FileContent>()
                .Where(x => x.Approved == true)
                .AsEnumerable();                                
        }
    }
}
