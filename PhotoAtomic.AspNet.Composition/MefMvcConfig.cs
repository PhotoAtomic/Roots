using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Http;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Mef.CommonServiceLocator;
using System.Configuration;

namespace PhotoAtomic.AspNet.Composition
{
    public static class MefMvcConfig
    {
       
        public static CompositionContainer RegisterMef(ComposablePartCatalog typeCatalog)
        {
            var compositionContainer = ConfigureContainer(typeCatalog);
            ServiceLocator
                .SetLocatorProvider(() => new MefServiceLocator(compositionContainer));

            ControllerBuilder
                .Current.SetControllerFactory(
                new MefMvcControllerFactory(compositionContainer));

            GlobalConfiguration
                .Configuration
                .DependencyResolver =
                new MefMvcDependencyResolver(compositionContainer);

            return compositionContainer;
        }

        private static CompositionContainer ConfigureContainer(
            ComposablePartCatalog composablePartCatalog)
        {

               

            var aggregateCatalog = new AggregateCatalog();

            if (composablePartCatalog != null)
            {
                aggregateCatalog.Catalogs.Add(composablePartCatalog);
            }

            return new CompositionContainer(
                aggregateCatalog,
                new MefNameValueCollectionExportProvider(
                    ConfigurationManager.AppSettings));
        }
    }

}
