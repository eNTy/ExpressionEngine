using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

using ExpressionEngine;

namespace Security.Builders
{
    [XmlType("UserField")]
    public class UserFieldBuilder : FieldBuilder
    {
        public override Expression Build()
        {
            ParameterExpression parameter = _parameters.Single(param => param.Name == "eUser").Build() as ParameterExpression;
            return BuildEFieldExpression(parameter);
        }
    }
}