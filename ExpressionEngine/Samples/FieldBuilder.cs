using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

using ExpressionEngine.Builders;
using DataLayer;

namespace Security.Builders
{
    public abstract class FieldBuilder : ExpressionBuilder
    {

        protected Expression BuildEFieldExpression(ParameterExpression eParam)
        {
            // field name constant
            ConstantExpression objectFieldName = Expression.Constant(Name, typeof(string));
            // get .net value
            MethodCallExpression methodValue = Expression.Call(eParam, "GetNetFieldValue", null, objectFieldName);

            // convert the column object value to proper data type
            Expression result = Expression.Convert(methodValue, Type);

            return result;
        }
    }
}