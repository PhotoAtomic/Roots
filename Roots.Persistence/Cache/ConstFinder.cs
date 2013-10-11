using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Persistence.Cache
{
    public class ConstFinder<T> : ExpressionVisitor
    {

        T constantValue;

        public T Find(Expression expression)
        {
            Visit(expression);
            return (T)constantValue;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {              
            if(node != null && typeof(T).IsAssignableFrom(node.Type))
            {
                object value = node.Value;
            }

            return base.VisitConstant(node);
        }
        

        protected override Expression VisitMember(MemberExpression node)
        {
            if(typeof(T).IsAssignableFrom(node.Type))
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
                            constantValue = (T)Expression.Lambda(node).Compile().DynamicInvoke();                            
                            break;
                        default:
                            throw new NotSupportedException();
                    }                    
                }
            }

            return base.VisitMember(node);
        }
    }
}
