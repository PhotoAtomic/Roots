using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.AspNet.Composition
{
    public class MefNameValueCollectionExportProvider : ExportProvider
    {
        private readonly List<Export> _exports;

        public MefNameValueCollectionExportProvider(NameValueCollection settings)
        {
            _exports = new List<Export>();

            foreach (string key in settings)
            {
                var metadata = new Dictionary<string, object> {
                {
                    CompositionConstants
                    .ExportTypeIdentityMetadataName, typeof (string)
                    .FullName
                }};

                var exportDefinition = new ExportDefinition(key, metadata);

                _exports.Add(new Export(exportDefinition, () => settings[key]));
            }
        }

        protected override IEnumerable<Export> GetExportsCore(
            ImportDefinition importDefinition, AtomicComposition atomicComposition)
        {
            return _exports
                .Where(x => importDefinition.IsConstraintSatisfiedBy(x.Definition));
        }
    }

}
