using PhotoAtomic.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            var constFinder = new ConstFinder<MemoryRepository<T>>();
            var repository = constFinder.Find(expression);
            repository.AggregateTrackedItem();

            var expressionDisassembler = new TypeDisassembler<IQueryable<T>>();
            expressionDisassembler.Disassemble(expression, repository);

            throw new NotImplementedException();
            //new ConstReplacer<MemoryRepository<T>

        }

        public object Execute(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

    
    }
}
