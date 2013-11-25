using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Extractors
{
    public class GetAllFileInSourcePath : Extractor<IEnumerable<string>>
    {
        public string Source { get; set; }
        public string Path { get; set; }

        protected override IEnumerable<string> PerformExtraction(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            var fileRepository = repositoryAccessor.RepositoryOf<Domain.FileContent>();
            return fileRepository
                .Where(x => x.Source == Source && x.SourceName.StartsWith(Path))
                .Select(x => x.SourceName)
                .ToList();
        }

    }
}
