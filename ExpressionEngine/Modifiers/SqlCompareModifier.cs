using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionEngine.Modifiers
{
    public class SqlCompareModifier : ExpressionVisitor
    {
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b.NodeType == ExpressionType.Equal || b.NodeType == ExpressionType.NotEqual)
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);

                // explicitely convert right to object
                Expression rightConverted = Expression.Convert(right, typeof(object));

                // use Equals() to compare
                MethodInfo[] methods = typeof(object).GetMethods();
                System.Reflection.MethodInfo info = left.Type.GetMethod("Equals", new Type[] { typeof(object) });
                Expression toReturn = Expression.Call(left, info, rightConverted);
                if (b.NodeType == ExpressionType.NotEqual)
                {
                    // negate
                    toReturn = Expression.MakeUnary(ExpressionType.Not, toReturn, typeof(bool));
                }
                return toReturn;
            }

            return base.VisitBinary(b);
        }
    }
}