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
    public abstract class RavenDbRespositoryBase<T> 
    {
        protected readonly Func<Type, PropertyInfo> getIdentityProperty;

        private readonly IsolationLevel isolationLevel;


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

        public RavenDbRespositoryBase(Func<Type, PropertyInfo> getIdentityProperty, IsolationLevel isolationLevel = IsolationLevel.None)
        {
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
                if (TryConvert(id, sourceType, targetType, out convertedValue))
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

        protected bool TryConvert(object value, Type sourceType, Type targetType, out object convertedValue)
        {
            convertedValue = null;
            var foundConventer = TypeDescriptor.GetConverter(targetType);
            if (foundConventer != null && foundConventer.CanConvertFrom(sourceType))
            {
                convertedValue = foundConventer.ConvertFrom(value);
                return true;
            }

            var foundReverseConventer = TypeDescriptor.GetConverter(sourceType);
            if (foundReverseConventer != null && foundReverseConventer.CanConvertTo(targetType))
            {
                convertedValue = foundConventer.ConvertTo(value,targetType);
                return true;
            }

            return false;
        }
    }
}
