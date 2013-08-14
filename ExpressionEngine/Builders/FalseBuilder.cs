using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("False")]
    public class FalseBuilder : ExpressionBuilder
    {
        public override Expression Build()
        {
            return Expression.Constant(false, typeof(bool));
        }
    }
}