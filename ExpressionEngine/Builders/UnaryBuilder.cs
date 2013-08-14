using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Unary")]
    public class UnaryBuilder : ComparisonBuilder
    {
        private ExpressionBuilder _builder;

        public ExpressionBuilder Builder
        {
            get { return _builder; }
            set { _builder = value; }
        }

        [XmlIgnore()]
        public Expression OperandExpression
        {
            get { return Builder.Build(_parameters); }
        }

        public override Expression Build()
        {
            Expression result = Expression.MakeUnary(ComparsionType, OperandExpression, Type);
            return result;
        }
    }
}
