using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("And")]
    public class AndBuilder : BinaryBuilder
    {
        public override Expression Build()
        {
            Expression result = Expression.MakeBinary(ExpressionType.And, LeftExpression, RightExpression);
            return result;
        }
    }
}