using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoAtomic.Extensions;
using PhotoAtomic.Reflection;
using Raven.Client;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Roots.Persistence.RavenDb.Cache
{
    class RavenDbQueryProviderForCache<T> : IQueryProvider
    {
        private IQueryProvider queryProvider;
        private IQueryable Repository;
        private bool async;

        public RavenDbQueryProviderForCache(IQueryProvider queryProvider, IQueryable Repository, bool async)
        {
            this.queryProvider = queryProvider;
            this.Repository = Repository;
            this.async = async;
        }

        public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
        {

            throw new NotImplementedException();
        }

        public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
        {
            return (TResult)Execute(expression);
        }

        public object Execute(System.Linq.Expressions.Expression expression)
        {
            if (expression.Type.IsEnumerable())
            {
                var sequenceType = Reflect.GetSequenceType(expression.Type);
                var method = typeof(Raven.Client.LinqExtensions)
                    .GetMethod(
                        Reflect.NameOf<object>(o => Raven.Client.LinqExtensions.ToListAsync<object>(null)));
                var genericMethod = method.MakeGenericMethod(sequenceType);
                var enumerableTask = genericMethod.Invoke(null, new object[] { Repository });

                var taskType = typeof(Task<>).MakeGenericType(typeof(IList<>).MakeGenericType(sequenceType));
                var result = taskType.GetProperty(Reflect.NameOf<Task<object>,object>(t => t.Result));

                var enumerable = result.GetValue(enumerableTask);

                return enumerable;
            }
            return queryProvider.Execute(expression);
        }
    }
}
