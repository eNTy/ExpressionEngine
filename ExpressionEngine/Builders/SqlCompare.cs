using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

using ExpressionEngine.Modifiers;

namespace ExpressionEngine.Builders
{
    [XmlType("SqlCompare")]
    public class SqlCompareBuilder : BinaryBuilder
    {
        public override Expression Build()
        {
            SqlCompareModifier modifier = new SqlCompareModifier();
            Expression original = Expression.MakeBinary(ComparsionType, LeftExpression, RightExpression);
            Expression result = modifier.Modify(original);
            return result;
        }
    }
}