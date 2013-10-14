using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    class SubExpressionChanger : ExpressionVisitor
    {
        private Expression sub;
        private Expression newSub;

        public Expression Change(Expression main, Expression sub, Expression newSub)
        {
            this.sub = sub;
            this.newSub = newSub;
            return Visit(main);
        }

        public override Expression Visit(Expression node)
        {
            if (node == sub)
            {
                return newSub;
            }
            return base.Visit(node);
        }
    }
}
