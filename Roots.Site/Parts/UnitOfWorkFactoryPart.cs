﻿using Roots.Persistence;
using Roots.Persistence.RavenDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace Roots.Site.Parts
{
    [Export (typeof(IUnitOfWorkFactory))]
    [Export(typeof(UnitOfWorkFactoryPart))]
    public class UnitOfWorkFactoryPart : IUnitOfWorkFactory
    {
        protected internal RavenDbUnitOfWorkFactory factory;

        public UnitOfWorkFactoryPart()
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["RootsDatabase"];
            if (connectionString == null) throw new Exception("connection string RootsDatabase not provided in web.config");
            //if (connectionString.ProviderName == "RavenDb")
            {
                factory = new RavenDbUnitOfWorkFactory(connectionString.ConnectionString);
                return;
            }
            //throw new Exception("Provider not supported: just ravendb provider is supported at the moment, sorry");
        }

        public IUnitOfWork CreateNew(IsolationLevel isolationLevel = IsolationLevel.None)
        {
            return factory.CreateNew(isolationLevel);
        }

        public IAsyncUnitOfWork CreateAsyncNew(IsolationLevel isolationLevel = IsolationLevel.None)
        {
            return factory.CreateAsyncNew(isolationLevel);
        }


    }
}