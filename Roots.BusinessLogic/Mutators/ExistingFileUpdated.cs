using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic.Mutators
{
    public class ExistingFileUpdated :  Mutator
    {
        public string Name { get; set; }

        public string MimeType { get; set; }

        public string NewName { get; set; }

        public byte[] FileContent { get; set; }

        public DateTime LastModify { get; set; }

        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {

            LastModify = DateTime.UtcNow;

            if (NewName == null || MimeType == null || FileContent == null) return;

            var file = repositoryAccessor.RepositoryOf<Domain.FileContent>().Where(x => x.Name == Name).SingleOrDefault();
            if(file == null){
                throw new FileNotFoundException("no file to update");
            }
            if(Name != null) file.Name = NewName;
            if(MimeType != null) file.MimeType = MimeType;
            if(FileContent != null) file.Data = FileContent;
            file.LastModify = LastModify;
        }
    }
}
