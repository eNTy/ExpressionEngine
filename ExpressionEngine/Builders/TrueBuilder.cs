using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("True")]
    public class TrueBuilder : ExpressionBuilder
    {
        public override Expression Build()
        {
            return Expression.Constant(true, typeof(bool));
        }
    }
}