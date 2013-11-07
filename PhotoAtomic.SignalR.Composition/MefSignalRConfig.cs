using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PhotoAtomic.SignalR.Composition;

namespace PhotoAtomic.AspNet.Composition
{
    public static class MefSignalRConfig
    {

        public static void RegisterMef(CompositionContainer container)
        {
            Microsoft.AspNet.SignalR.GlobalHost.DependencyResolver = new MefDependencyResolver(container);
        }
    }

}
