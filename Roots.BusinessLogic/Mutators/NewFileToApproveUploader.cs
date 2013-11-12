using Roots.BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Mutators
{
    public class NewFileToApproveUploader : Mutator
    {
        public byte[] FileContent { get; set; }
        public string Source { get; set; }
        public string SourceName { get; set; }
        public string MimeType { get; set; }
        public DateTime CreateDate { get; set; }


        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            if (string.IsNullOrWhiteSpace(Source)) throw new ArgumentException(Source);
            CreateDate = DateTime.UtcNow;
            if (repositoryAccessor.RepositoryOf<Domain.FileContent>().Any(x => x.Source == Source && x.SorceName == SourceName)) throw new FileAlreadyExistsException();
            var files = repositoryAccessor.RepositoryOf<Domain.FileContent>();
            files.Add(new Domain.FileContent
            {
                Approved = false,
                Data = FileContent,
                Source = Source,
                SorceName = SourceName,
                MimeType = MimeType,
                CreateDate = CreateDate,
                LastModify = CreateDate,
            });
        }
    }
}
