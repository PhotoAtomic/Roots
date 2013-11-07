using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic.Mutators
{
    public class ExistingFileRenamed : Mutator
    {
        public string OldName { get; set; }

        public string NewName { get; set; }

        public DateTime LastModify { get; set; }
        
        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            var file = repositoryAccessor.RepositoryOf<Domain.FileContent>().Where(x => x.Name == OldName).SingleOrDefault();
            if(file == null){
                throw new FileNotFoundException("no file to update");
            }
            file.Name = NewName;            
            file.LastModify = LastModify;
        }
    }
}
