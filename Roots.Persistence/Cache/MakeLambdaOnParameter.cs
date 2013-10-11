using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    public class MakeLambdaOnParameter<T> : ExpressionVisitor
    {
        ParameterExpression parameter;
        public LambdaExpression Replace(Expression expression)
        {
            parameter = Expression.Parameter(typeof(T), "paramX");
            var replaced = Visit(expression);
            return Expression.Lambda(replaced, parameter);
        }


        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node != null && typeof(T).IsAssignableFrom(node.Type))
            {
                return parameter;
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
                            return parameter;                            
                        default:
                            throw new NotSupportedException();
                    }
                }
            }

            return base.VisitMember(node);
        }

    }
}
