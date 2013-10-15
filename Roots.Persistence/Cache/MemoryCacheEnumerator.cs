using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoAtomic.Extensions;
using System.Reflection;


namespace Roots.Persistence.Cache
{
    class MemoryCacheEnumerator<T> : IEnumerator<T>
    {
        public IEnumerator<T> internalEnumerator;
        private IDisposable[] disposables;
        private Action<T> track;

        public MemoryCacheEnumerator(IEnumerable<T> localEnum, IEnumerable<T> remoteEnum, ISet<object> evicted, Action<T> track, PropertyInfo idProperty, params IDisposable[] disposables)
        {
            this.disposables = disposables;
            this.track = track;

            var enumeratorExpression= 
                localEnum
                .Union(remoteEnum,
                    new ItemEqualityComparer<T>(
                        (x, y) => idProperty.GetValue(x) == idProperty.GetValue(y),
                        x =>
                        {
                            var value = idProperty.GetValue(x);
                            if (value == null) return 0;
                            return value.GetHashCode();
                        }));
            if (evicted != null)
            {
                enumeratorExpression = enumeratorExpression.Where(x => !evicted.Contains(idProperty.GetValue(x)));
            }
            internalEnumerator = enumeratorExpression.GetEnumerator();
        }

        public T Current
        {
            get
            {
                return internalEnumerator.Current;
            }
            
        }

        public void Dispose()
        {
            if (disposables == null) return;
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {            
            var hasNext = internalEnumerator.MoveNext();
            if (!hasNext)
            {             
                return false;
            }
            track(internalEnumerator.Current);
            return hasNext;
        }

        public void Reset()
        {
            internalEnumerator.Reset();
        }

        
    }
}
