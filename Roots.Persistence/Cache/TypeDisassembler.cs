using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Roots.Persistence.Cache
{
    public class TypeDisassembler<T> : ExpressionVisitor
    {
        public Expression FirstPart { get; private set; }
        public Expression SecondPart { get; private set; }

        private T root;

        public void Disassemble(Expression ex, T root)
        {
            this.root = root;
            Visit(ex);
        }

        public override Expression Visit(Expression node)
        {
            if (node != null)
            {
                if (typeof(T).IsAssignableFrom(node.Type))
                {
                    if (SecondPart == null)
                    {                        
                        var finder = new ConstFinder<T>();
                        var constValue = finder.Find(node);

                        if (constValue != null && constValue.Equals(root))
                        {
                            SecondPart = node;
                        }
                    }
                }
                else
                {
                    if (FirstPart == null) FirstPart = node;
                }
            }
            return base.Visit(node);
        }

    }
}
