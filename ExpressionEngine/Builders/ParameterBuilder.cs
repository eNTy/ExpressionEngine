using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Parameter")]
    public class ParameterBuilder : ExpressionBuilder
    {
        ParameterExpression _expression;

        public ParameterExpression Parameter
        {
            get
            {
                if (_expression == null)
                {
                    _expression = Expression.Parameter(Type, Name);
                }
                return _expression;
            }
        }

        public KeyValuePair<string, ParameterExpression> GetPair()
        {
            return new KeyValuePair<string, ParameterExpression>(this.Name, this.Parameter);
        }

        /// <summary>
        /// Parameters are compared using instance reference, we must re-use the same expression
        /// </summary>
        /// <returns></returns>
        public override Expression Build()
        {
            return Parameter;
        }
    }
}