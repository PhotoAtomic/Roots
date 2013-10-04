using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAtomic.AspNet.Composition
{
    public class MefAdapter<T1, T2, TResult>
    {
        private static readonly Func<T1, T2, TResult>
            CreateExport = Create<T1, T2, TResult>();

        private readonly TResult _typeToExport;

        [ImportingConstructor]
        public MefAdapter(T1 arg1, T2 arg2)
        {
            _typeToExport = CreateExport(arg1, arg2);
        }

        [Export]
        public virtual TResult TypeToExport
        {
            get { return _typeToExport; }
        }

        internal static Func<T1, T2, TResult> Create<T1, T2, TResult>()
        {
            var constructorArgExpression1 = Expression.Parameter(typeof(T1), "arg1");
            var constructorArgExpression2 = Expression.Parameter(typeof(T2), "arg2");

            var constructorInfo = typeof(TResult).GetConstructor(new[]
                {
                    typeof (T1),
                    typeof (T2)
                });

            var constructorExpression = Expression
                .New(constructorInfo, constructorArgExpression1, constructorArgExpression2);

            return Expression.Lambda<Func<T1, T2, TResult>>(
                constructorExpression,
                constructorArgExpression1,
                constructorArgExpression2)
                .Compile();
        }
    }

}
