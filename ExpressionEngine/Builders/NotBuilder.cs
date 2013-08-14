using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Not")]
    public class NotBuilder : UnaryBuilder
    {
        public override Expression Build()
        {
            Expression result = Expression.MakeUnary(ExpressionType.Not, OperandExpression, typeof(bool));
            return result;
        }
    }
}