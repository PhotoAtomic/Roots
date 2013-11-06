using PhotoAtomic.AspNet.Composition;
using Roots.BusinessLogic;
using Roots.Persistence;
using Roots.Site.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Roots.Site
{
    public class CompositionConfig
    {
        public static void RegisterComposition()
        {
            // Set up all the Mef conventions for our web assembly
            var registrationBuilder = new RegistrationBuilder();

            registrationBuilder.ForTypesDerivedFrom<Controller>()
                .SetCreationPolicy(CreationPolicy.NonShared).Export();

            registrationBuilder.ForTypesDerivedFrom<ApiController>()
                .SetCreationPolicy(CreationPolicy.NonShared).Export();

            //registrationBuilder
            //    .ForTypesMatching(t =>
            //        t.FullName.StartsWith(
            //            Assembly
            //                .GetExecutingAssembly()
            //                .GetName().Name + ".Parts"))
            //    .SetCreationPolicy(CreationPolicy.NonShared)
            //    .ExportInterfaces(x => x.IsPublic);


            registrationBuilder
                .ForType<UnitOfWorkFactoryPart>()
                .SetCreationPolicy(CreationPolicy.Shared)
                .ExportInterfaces(x => x.IsPublic);
             

            //registrationBuilder
            //    .ForType<UnitOfWorkFactoryForCachePart>()
            //    .SetCreationPolicy(CreationPolicy.Shared)
            //    .ExportInterfaces(x => x.IsPublic);
           
            var aggregateCatalog = new AggregateCatalog();

            var typeCatalog = new TypeCatalog(
                typeof(DomainSupervisor)
            );

            aggregateCatalog.Catalogs.Add(
                new AssemblyCatalog(Assembly.GetExecutingAssembly(), registrationBuilder));
            aggregateCatalog.Catalogs.Add(
                typeCatalog);
                                    
            MefMvcConfig.RegisterMef(aggregateCatalog);                     
        }
    }
}