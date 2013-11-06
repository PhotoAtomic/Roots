using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Mutators
{
    public class NewFileUploaded : Mutator
    {

        public byte[] FileContent {get;set;}
        public string Name { get; set; }
        public string MimeType { get; set; }


        protected override void ApplyMutation(RepositoryAccessor repositoryAccessor)
        {
            var files = repositoryAccessor.RepositoryOf<Domain.FileContent>();
            files.Add(new Domain.FileContent
            {
                Data = FileContent,
                Name = Name,
                MimeType = MimeType,
            });
        }
    }
}
