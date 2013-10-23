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
                var enumerableTask = (Task)genericMethod.Invoke(null, new object[] { Repository });

                var taskType = typeof(Task<>).MakeGenericType(typeof(IList<>).MakeGenericType(sequenceType));
                var result = taskType.GetProperty(Reflect.NameOf<Task<object>,object>(t => t.Result));


                throw new NotImplementedException("i have to stop here because it doesn't look there is an obvious way to make the enumerableTask Task to run as usually this method is called in a continuation of a task, maybe we should use a different scheduler? i don't know. a tthe moemtn i don't have time to investigate, Test works by removing this exception because they are not executed from a continuation task, anyway real code will not");
                
                var enumerable = result.GetValue(enumerableTask);

                return enumerable;
            }
            return queryProvider.Execute(expression);
        }
    }
}
