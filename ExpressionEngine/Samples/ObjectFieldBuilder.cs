using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

using ExpressionEngine;

namespace Security.Builders
{
    [XmlType("ObjectField")]
    public class ObjectFieldBuilder : FieldBuilder
    {
        public override Expression Build()
        {
            ParameterExpression parameter = _parameters.Single(param => param.Name == "eObject").Build() as ParameterExpression;
            return BuildEFieldExpression(parameter);
        }
    }
}