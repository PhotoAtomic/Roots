using Roots.BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Mutators
{
    public class NewFileUploader : Mutator
    {

        public byte[] FileContent {get;set;}
        public string Source { get; set; }
        public string SourceName { get; set; }
        public string MimeType { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid IdOfTheNewFile { get; set; }


        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            if (string.IsNullOrWhiteSpace(Source)) throw new ArgumentException(Source);
            CreateDate = DateTime.UtcNow;
            if (repositoryAccessor.RepositoryOf<Domain.FileContent>().Any(x =>x.Source == Source && x.SorceName == SourceName)) throw new FileAlreadyExistsException();
            var files = repositoryAccessor.RepositoryOf<Domain.FileContent>();
            var newFile = new Domain.FileContent
            {
                Approved = true,
                Data = FileContent,
                Source = Source,
                SorceName = SourceName,
                MimeType = MimeType,
                CreateDate = CreateDate,
                LastModify = CreateDate,
            };
            files.Add(newFile);
            IdOfTheNewFile = newFile.Id;
        }
    }
}
