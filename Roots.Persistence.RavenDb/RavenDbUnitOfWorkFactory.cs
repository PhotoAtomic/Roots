using PhotoAtomic.Reflection;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Document;
using Roots.Persistence.RavenDb.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb
{
    public class RavenDbUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IDocumentStore documentStore;
        
        public RavenDbUnitOfWorkFactory(string connectionString)
        {
            documentStore = MakeDocumentStore(connectionString);

            documentStore.Conventions.CustomizeJsonSerializer = x =>
            {
                x.ReferenceLoopHandling = Raven.Imports.Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                x.PreserveReferencesHandling = Raven.Imports.Newtonsoft.Json.PreserveReferencesHandling.All;
            };

            SetupKeyTypeLockRule(documentStore);

            documentStore.Initialize();
            
        }

        protected virtual IDocumentStore MakeDocumentStore(string connectionString)
        {
            return new DocumentStore { Url = connectionString };
        }

        public static void SetupKeyTypeLockRule(IDocumentStore documentStore)
        {
            var keyLockedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Select(t => new
                    {
                        Type = t,
                        AttributesType = t.GetCustomAttributes(false).Select(a => a.GetType())
                    })
                .Where(x => x.AttributesType.Contains(typeof(KeyTypeLockAttribute)));

            foreach (var keyLockedType in keyLockedTypes)
            {

                string typeName = keyLockedType.Type.Name;
                PropertyInfo identityProperty = documentStore.Conventions.GetIdentityProperty(keyLockedType.Type);
                string separator = (identityProperty == null) ? "/" : documentStore.Conventions.IdentityPartsSeparator;
                Func<string, IDatabaseCommands, object, string> f = (s, c, o) => string.Format("{0}{1}{2}", typeName, separator, GetId(o, identityProperty));

                Reflect
                    .CallGenericOn<DocumentConvention>(x => x.RegisterIdConvention<object>(null), keyLockedType.Type)
                    .Invoke(documentStore.Conventions, new object[] { f });
            }
        }

        private static object GetId(object o, PropertyInfo identityProperty)
        {
            if (o == null) return null;
            if (identityProperty == null) return null;
            return identityProperty.GetValue(o);            
        }

        public IUnitOfWork CreateNew(IsolationLevel isolationLevel = IsolationLevel.None)
        {
            return new RavenDbUnitOfWork(documentStore, isolationLevel);
        }


        public Roots.Persistence.IAsyncUnitOfWork CreateAsyncNew(IsolationLevel isolationLevel = IsolationLevel.None)
        {
            return new RavenDbAsyncUnitOfWork(documentStore, isolationLevel);
        }   
    }
}
