using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.Extensions
{
    public static class TypeExtensions
    {
        public static int? SpecificityDistance(this Type descendantType, Type baseType)
        {

            if (!baseType.IsAssignableFrom(descendantType)) return null;           
            if (descendantType == baseType) return 0;
            if (descendantType.IsInterface && !baseType.IsInterface) return null;

            var min = descendantType
                .GetInterfacesOnType()
                .Select(x => x.SpecificityDistance(baseType))
                .Union(descendantType.BaseType.SpecificityDistance(baseType).AsEnumerable())
                .Where(x=>x.HasValue)
                .Min();

            if (min.HasValue) return min.Value + 1;
            return null;
            
        }

        public static IEnumerable<Type> GetInterfacesOnType(this Type type)
        {
            return type.GetInterfaces().Where(x => !x.IsAssignableFrom(type.BaseType));
        }
    }
}
