using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace PhotoAtomic.AspNet.Composition
{
    public class MefMvcDependencyResolver : IDependencyResolver
    {
        private readonly CompositionContainer _compositionContainer;

        public MefMvcDependencyResolver(CompositionContainer compositionContainer)
        {
            _compositionContainer = compositionContainer;
        }

        #region IDependencyResolver Members

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type type)
        {
            var export = _compositionContainer
                .GetExports(type, null, null).SingleOrDefault();

            return null != export ? export.Value : null;
        }

        public IEnumerable<object> GetServices(Type type)
        {
            var exports = _compositionContainer.GetExports(type, null, null);
            var exportList = new List<object>();
            if (exports.Any()) exportList.AddRange(exports.Select(export => export.Value));
            return exportList;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
