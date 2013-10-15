using PhotoAtomic.Reflection;
using PhotoAtomic.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Roots.Persistence.Cache
{
    class MemoryCacheQueryProvider<T> : IQueryProvider
    {
        

        public MemoryCacheQueryProvider()
        {
        
        }

        public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
        {

            return new MemoryQueryable<TElement>(expression, this);

        }

        public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
        {
            Type elementType = Reflect.GetSequenceType(expression.Type);
            if (elementType == null) elementType = expression.Type;

            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(MemoryQueryable<>).MakeGenericType(elementType), new object[] { expression, this });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
        {
            return (TResult)Execute(expression);
           
        }

        public object Execute(System.Linq.Expressions.Expression expression)
        {
            var constFinder = new ConstFinder<MemoryRepository<T>>();
            var repository = constFinder.Find(expression);
            repository.AggregateTrackedItem();

            var expressionDisassembler = new TypeDisassembler<IQueryable<T>>();
            expressionDisassembler.Disassemble(expression, repository);

            var where = expressionDisassembler.SecondPart;
            var select = expressionDisassembler.FirstPart;

            var constantChanger = new ChangeConstant<IQueryable<T>>();

            var sessionInfo = repository.GetRepositoryAsQueryable();
            var remoteSession = sessionInfo.Item1;
            var remoteRepository = sessionInfo.Item2;
            var remoteWhere = constantChanger.Replace(where, remoteRepository);
            var remoteEnum = remoteRepository.Provider.Execute<IEnumerable<T>>(remoteWhere);

            var localCache = repository.GetCacheAsQueryable();
            var localWhere = constantChanger.Replace(where, localCache);
            var localEnum = localCache.Provider.Execute<IEnumerable<T>>(localWhere);

            var enumerator = new MemoryCacheEnumerator<T>(
                localEnum,
                remoteEnum,
                repository.idToRemove,
                x => repository.Track(x),                
                repository.GetIdProperty(),
                remoteSession);

            var queryable = enumerator.ToEnumerable().AsQueryable();

            var changer = new SubExpressionChanger();
            var selectExpression = changer.Change(select, where, Expression.Constant(queryable));
            var result = queryable.Provider.Execute(selectExpression);
            return result;
        }

        

    
    }
}
