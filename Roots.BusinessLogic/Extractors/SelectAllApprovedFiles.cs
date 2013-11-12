using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.BusinessLogic.Extractors
{
    public class SelectAllApprovedFiles : Selector<IEnumerable<FilePreview>>
    {
        protected override IEnumerable<FilePreview> PerformExtraction(ReadOnlyRepositoryAccessor repositoryAccessor)
        {
            return repositoryAccessor.RepositoryOf<Domain.FileContent>()
                .Where(x => x.Approved == true)
                .AsEnumerable()
                .Select(x => new FilePreview
                {
                    Id = x.Id,
                    Name = x.SorceName,
                    MimeType = x.MimeType,
                    Content = (x.MimeType == "chemical/x-mdl-sdf") ? x.Data : null,
                });
                
        }
    }
}
