using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Extractors
{
    public class GetFileContent : Extractor<Domain.FileContent>
    {
        public Guid Id { get; set; }

        protected override Domain.FileContent PerformExtraction(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            var result = repositoryAccessor.RepositoryOf<Domain.FileContent>().GetById(Id);

            return result;
        }
    }
}
