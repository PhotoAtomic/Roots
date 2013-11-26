using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Mutators
{
    public class ExistingFileTypeChanger : Mutator
    {
        public string Source { get; set; }

        public string SourceName { get; set; }

        public string NewType { get; set; }
        

        public Guid IdOfTheChangedFile { get; set; }

        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            if (string.IsNullOrWhiteSpace(Source)) throw new ArgumentException(Source);

            var file = repositoryAccessor.RepositoryOf<Domain.FileContent>().Where(x => x.Source == Source && x.SourceName == SourceName).SingleOrDefault();
            if (file == null)
            {
                throw new FileNotFoundException("no file to update");
            }
            IdOfTheChangedFile = file.Id;
            file.MimeType = NewType;
            file.LastModify = DateTime.UtcNow;
        }
    }
}
