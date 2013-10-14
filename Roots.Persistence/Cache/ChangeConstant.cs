using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    public class ChangeConstant<T> : ExpressionVisitor
    {
        ConstantExpression newConstant;
        public Expression Replace(Expression expression,T newConstantValue)
        {
            newConstant = Expression.Constant(newConstantValue,typeof(T));
            return Visit(expression);
            
        }


        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node != null && typeof(T).IsAssignableFrom(node.Type))
            {
                return newConstant;
            }

            return base.VisitConstant(node);
        }


        protected override Expression VisitMember(MemberExpression node)
        {
            if (typeof(T).IsAssignableFrom(node.Type))
            {
                if (!typeof(T).IsAssignableFrom(node.Expression.Type))
                {
                    if (node.Expression.NodeType != ExpressionType.Constant) throw new NotSupportedException("can access to just constant nodes");
                    object constantObject = ((ConstantExpression)node.Expression).Value;
                    if (constantObject == null) throw new NullReferenceException("constant object is null");
                    Type constantObjectType = node.Expression.Type;
                    var member = node.Member;
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                        case MemberTypes.Property:
                            return newConstant;                            
                        default:
                            throw new NotSupportedException();
                    }
                }
            }

            return base.VisitMember(node);
        }

    }
}
