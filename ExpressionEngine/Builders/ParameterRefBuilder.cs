using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("ParameterRef")]
    public class ParameterRefBuilder : ExpressionBuilder
    {  
        public override Expression Build()
        {
            Expression result = _parameters.Single(param => param.Name == Name).Build() as ParameterExpression;
            return result;
        }
    }
}