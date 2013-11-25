using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic.Mutators
{
    public class ExistingFileRenamer : Mutator
    {
        public string Source { get; set; }

        public string OldSourceName { get; set; }

        public string NewSourceName { get; set; }

        public DateTime LastModify { get; set; }

        public Guid IdOfTheRenamedFile { get; set; }
        
        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            if (string.IsNullOrWhiteSpace(Source)) throw new ArgumentException(Source);

            var file = repositoryAccessor.RepositoryOf<Domain.FileContent>().Where(x =>x.Source == Source && x.SourceName == OldSourceName).SingleOrDefault();
            if(file == null){
                throw new FileNotFoundException("no file to update");
            }
            IdOfTheRenamedFile = file.Id;
            file.SourceName = NewSourceName;            
            file.LastModify = LastModify;            
        }
    }
}
