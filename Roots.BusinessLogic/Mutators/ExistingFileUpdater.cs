using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roots.BusinessLogic.Mutators
{
    public class ExistingFileContentUpdated : Mutator
    {
        public string Source { get; set; }

        public string SourceName { get; set; }

        public string MimeType { get; set; }
        
        public byte[] FileContent { get; set; }

        public DateTime LastModify { get; set; }

        public Guid IdOfUpdatedFile { get; set; }

        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            if (string.IsNullOrWhiteSpace(Source)) throw new ArgumentException(Source);

            LastModify = DateTime.UtcNow;

            if (MimeType == null || FileContent == null) return;

            var file = repositoryAccessor.RepositoryOf<Domain.FileContent>().Where(x => x.Source == Source && x.SourceName == SourceName).SingleOrDefault();
            if (file == null)
            {
                throw new FileNotFoundException("no file to update");
            }           
            if (MimeType != null) file.MimeType = MimeType;
            if (FileContent != null) file.Data = FileContent;
            file.LastModify = LastModify;
            IdOfUpdatedFile = file.Id;
        }
    }
}
