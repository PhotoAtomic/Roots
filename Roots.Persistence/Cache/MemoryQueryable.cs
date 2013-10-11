using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    class MemoryQueryable<T> : IQueryable<T>
    {

        public MemoryQueryable(Expression expression, IQueryProvider provider)
        {
            Provider = provider;
            Expression = expression;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get;
            private set;
        }

        public IQueryProvider Provider
        {
            get;
            private set;
        }
    }
}
