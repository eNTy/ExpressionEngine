using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Property")]
    public class PropertyBuilder : UnaryBuilder
    {        
        public override Expression Build()
        {
            Expression result = Expression.PropertyOrField(OperandExpression, Name);
            return result;
        }
    }
}