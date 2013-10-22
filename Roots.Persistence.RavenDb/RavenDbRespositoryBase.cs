using PhotoAtomic.Reflection;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.RavenDb
{
    public abstract class RavenDbRespositoryBase<T>: IIdGenerator
    {
        protected readonly Func<Type, PropertyInfo> getIdentityProperty;

        private readonly IsolationLevel isolationLevel;

        private GenerateEntityIdOnTheClient keyGenerator;

        private IRavenQueryable<T> repository;
        protected IRavenQueryable<T> Repository
        {
            get
            {
                if (repository == null)
                {

                    if (isolationLevel == IsolationLevel.None)
                    {
                        repository = GetRavenQueryable();
                    }
                    else
                    {
                        repository = GetRavenQueryable()
                            .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite());
                    }
                }
                return repository;
            }
        }

        protected abstract IRavenQueryable<T> GetRavenQueryable();

        public RavenDbRespositoryBase(IDocumentStore documentStore, Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel = IsolationLevel.None)
        {
            this.documentStore = documentStore;
            this.isolationLevel = isolationLevel;
            this.getIdentityProperty = getIdentityProperty;            
        }


        protected TOut InvokeByTargetType<TOut>(object id, Func<string, TOut> methodWithStringId, Func<ValueType, TOut> methodWithValueTypeId)
        {
            var sourceType = id.GetType();
            var targetType = getIdentityProperty(typeof(T)).PropertyType;

            if (sourceType == targetType)
            {
                if (sourceType == typeof(string))
                {
                    return methodWithStringId((string)id);// documentSession.LoadAsync<T>((string)id);
                }
                if (sourceType.IsValueType)
                {
                    return methodWithValueTypeId((ValueType)id);//documentSession.LoadAsync<T>((ValueType)id);
                }
            }
            else
            {
                object convertedValue;
                if (Conversion.Try(id, sourceType, targetType, out convertedValue))
                {
                    if (targetType == typeof(string))
                    {
                        return methodWithStringId((string)convertedValue);
                    }
                    if (targetType.IsValueType)
                    {
                        return methodWithValueTypeId((ValueType)convertedValue);
                    }
                }
            }

            throw new Exception(
                String.Format(
                    "Can't find a converter for property id from type {0} to type {1}",
                    sourceType.Name,
                    targetType.Name));
        }



        public PropertyInfo GetIdProperty()
        {
            return getIdentityProperty(typeof(T));
        }

        protected string KeyForId(object id)
        {
            object instance = Activator.CreateInstance<T>();
            GetIdProperty().SetValue(instance, id);            
            
            string key;
            GetKeyGenerator(documentStore).TryGetIdFromInstance(instance,out key);
            return key;
        }

        private GenerateEntityIdOnTheClient GetKeyGenerator(IDocumentStore store)
        {
            if (keyGenerator != null) return keyGenerator;
            keyGenerator = new GenerateEntityIdOnTheClient(store, x => store.Conventions.GenerateDocumentKey(store.Identifier, store.DatabaseCommands, x));
            return keyGenerator;
        }



        public object GenerateNewId()
        {
            var instance = Activator.CreateInstance<T>();
            string id;
            var idProperty = GetIdProperty();
            if (!GetKeyGenerator(documentStore).TryGetIdFromInstance(instance, out id))
            {
                return idProperty.GetValue(instance);
            }
            string justId = id.Split(new string[]{documentStore.Conventions.IdentityPartsSeparator}, StringSplitOptions.RemoveEmptyEntries).Last();
            
            

            object convertedId;
            if (Conversion.Try(justId, justId.GetType(), idProperty.PropertyType, out convertedId)) return convertedId;
            return idProperty.GetValue(instance);
        }

        public IDocumentStore documentStore { get; set; }

        public abstract void Dispose();
    }
}
