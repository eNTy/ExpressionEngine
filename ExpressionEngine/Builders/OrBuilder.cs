using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Or")]
    public class OrBuilder : BinaryBuilder
    {
        public override Expression Build()
        {
            Expression result = Expression.MakeBinary(ExpressionType.Or, LeftExpression, RightExpression);
            return result;
        }
    }
}