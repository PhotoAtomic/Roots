using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.Composition;

namespace PhotoAtomic.AspNet.Composition
{
    public class MefMvcControllerFactory : DefaultControllerFactory
    {
        private readonly CompositionContainer _compositionContainer;

        public MefMvcControllerFactory(CompositionContainer compositionContainer)
        {
            _compositionContainer = compositionContainer;
        }

        protected override IController GetControllerInstance(
            RequestContext requestContext, Type controllerType)
        {
            var export = _compositionContainer
                .GetExports(controllerType, null, null).SingleOrDefault();

            IController result;

            if (null != export)
                result = export.Value as IController;
            else
            {
                result = base.GetControllerInstance(requestContext, controllerType);
                _compositionContainer.ComposeParts(result);
            }

            return result;
        }

    }
}
