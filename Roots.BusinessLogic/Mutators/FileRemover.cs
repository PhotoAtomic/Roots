using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Mutators
{
    public class FileRemover : Mutator
    {
        public string Source { get; set; }
        public string SourceName { get; set; }
        public Guid IdOfTheRemovedFiles { get; set; }

        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            var fileRepository = repositoryAccessor.RepositoryOf<Domain.FileContent>();
            var file = fileRepository.Where(x =>x.Source == Source && x.SourceName == SourceName).SingleOrDefault();
            IdOfTheRemovedFiles = file.Id;
            fileRepository.Remove(file);
        }
        
    }
}
