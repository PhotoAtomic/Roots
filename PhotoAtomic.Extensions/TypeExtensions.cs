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
            if (descendantType == null || baseType == null) return null;

            if (descendantType == baseType) return 0;

            if (descendantType.IsGenericType && baseType.IsGenericType)
            {                
                if (baseType.IsGenericTypeDefinition)
                {
                    if (descendantType.GetGenericTypeDefinition() == baseType) return 1;
                    //else throw new NotSupportedException();
                }
                else if (descendantType.GetGenericTypeDefinition() == baseType.GetGenericTypeDefinition())
                {
                    var distances = Enumerable.Zip(
                        descendantType.GetGenericArguments(),
                        baseType.GetGenericArguments(),
                        (t1, t2) => new { GenericDescArg = t1, GenericBaseArg = t2 })
                        .Select(args => SpecificityDistance(args.GenericDescArg, args.GenericBaseArg));

                    if (distances.All(x => x != null))
                    {
                        return distances.Sum() + 1;
                    }
                }
            }


            if (!baseType.IsAssignableFrom(descendantType)) return null;                       
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

        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterfaces()
                .Select(x => x.IsGenericType ? x.GetGenericTypeDefinition() : x)
                .Any(x => x == typeof(IEnumerable<>));
        }
    }
}
