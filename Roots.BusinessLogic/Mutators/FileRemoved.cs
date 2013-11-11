using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Mutators
{
    public class FileRemoved : Mutator
    {
        public string Source { get; set; }
        public string SourceName { get; set; }

        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            var fileRepository = repositoryAccessor.RepositoryOf<Domain.FileContent>();
            var files = fileRepository.Where(x =>x.Source == Source && x.SorceName == SourceName);
            foreach (var file in files)
            {
                fileRepository.Remove(file);
            }
        }
        
    }
}
