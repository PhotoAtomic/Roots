using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Selectors
{
    public class GetAllFileInSourcePath : Selector<IEnumerable<string>>
    {
        public string Source { get; set; }
        public string Path { get; set; }

        protected override IEnumerable<string> PerformSelection(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            var fileRepository = repositoryAccessor.RepositoryOf<Domain.FileContent>();
            return fileRepository
                .Where(x => x.Source == Source && x.SorceName.StartsWith(Path))
                .Select(x => x.SorceName)
                .ToList();
        }

    }
}
