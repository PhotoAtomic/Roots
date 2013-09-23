using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.Extensions
{
    public static class TypeExtensions
    {
        public static int? Specificity(this Type descendantType, Type baseType)
        {
            
            if(!baseType.IsAssignableFrom(descendantType)) return null;

            int? interfacePathLength = null;
            if(baseType.IsInterface){
                if (baseType.IsAssignableFrom(descendantType) &&
                   !baseType.IsAssignableFrom(descendantType.BaseType))
                {
                    interfacePathLength = 1;
                }
                else
                {
                    interfacePathLength = descendantType.BaseType.Specificity(baseType) + 1;
                }

                var minInterfacePath = descendantType
                    .GetInterfaces()
                    .Select(x => x.Specificity(baseType))
                    .Where(x => x.HasValue);
                   
                    ////COMPLETE INTERFACES

                //if (minInterfacePath.HasValue && minInterfacePath < interfacePathLength) minInterfacePath = interfacePathLength;
            }

            if (descendantType == baseType) return 0;

            int? classPathLength = descendantType.BaseType.Specificity(baseType) + 1;

            if (interfacePathLength.HasValue && classPathLength.HasValue) return Math.Min(classPathLength.Value,interfacePathLength.Value);
            if (interfacePathLength.HasValue) return interfacePathLength.Value;
            if (classPathLength.HasValue) return classPathLength.Value;
            return null;
        }
    }
}
