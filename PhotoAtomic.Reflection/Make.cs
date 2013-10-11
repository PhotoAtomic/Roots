using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.Reflection
{
    public static class Make
    {

        public class GenericDescriptor<T>
        {
            public GenericDescriptor(Type type)
            {
                this.type = type;
            }
            public Type type;
        }

        public static GenericDescriptor<T> WithGenericParameters<T>(params Type[] types)
        {
            Type requestedType = typeof(T);
            Type typeToConstruct;
            if (!requestedType.IsGenericType && 
                types != null && 
                types.Length != 0)
            {
                typeToConstruct = requestedType.GetGenericTypeDefinition().MakeGenericType(types);
            }
            else
            {
                typeToConstruct = requestedType;
            }

            return new GenericDescriptor<T>(typeToConstruct);            
        }

        public static T Activate<T>(this GenericDescriptor<T> type, params object[] args)
        {            
            return (T)Activator.CreateInstance(type.type, args);
        }
    }
}
