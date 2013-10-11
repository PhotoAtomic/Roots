using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoAtomic.Extensions
{
    public class ItemEqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> comparer;
        private Func<T, int> hashGenerator;

        public ItemEqualityComparer(Func<T, T, bool> comparer = null, Func<T,int> hashGenerator = null)
        {            
            this.comparer = comparer;
            this.hashGenerator = hashGenerator;
        }

        public bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            if (x == null ^ y == null) return false;
            if(comparer != null) return comparer(x,y);

            var xEqualT = x as IEquatable<T>;
            if (xEqualT != null) return xEqualT.Equals(y);
            var xEqual = x as IEquatable;
            if (xEqual != null) return xEqual.Equals(y);                  
            return Equals(x, y);                        
        }

        public int GetHashCode(T obj)
        {
            if (hashGenerator != null) return hashGenerator(obj);
            if (obj == null) return 0;
            return obj.GetHashCode();
        }



      
    }
}
