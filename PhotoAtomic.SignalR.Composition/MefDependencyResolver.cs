using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;

namespace PhotoAtomic.SignalR.Composition
{
    class MefDependencyResolver : DefaultDependencyResolver
    {
        private CompositionContainer container;

        public MefDependencyResolver(CompositionContainer container)
        {
            this.container = container;
        }


        public override object GetService(Type serviceType)
        {

            var service = container.GetExports(serviceType, null, null).SingleOrDefault();
            if (service != null)
            {
                return service.Value;
            }
            return base.GetService(serviceType);


        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Concat(
                container.GetExports(serviceType, null, null).Select(x=>x.Value),
                base.GetServices(serviceType));
        }
    }
}
