using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Equals")]
    public class EqualsBuilder : BinaryBuilder
    {
        public override Expression Build()
        {
            Expression result = Expression.MakeBinary(ExpressionType.Equal, LeftExpression, RightExpression);
            return result;
        }
    }
}