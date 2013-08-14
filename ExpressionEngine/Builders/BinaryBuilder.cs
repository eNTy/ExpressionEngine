using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Binary")]
    public class BinaryBuilder : ComparisonBuilder
    {
        protected ExpressionSide _left;
        protected ExpressionSide _right;      

        [XmlElement("Left")]
        public ExpressionSide Left
        {
            get { return _left; }
            set { _left = value; }
        }

        [XmlElement("Right")]
        public ExpressionSide Right
        {
            get { return _right; }
            set { _right = value; }
        }

        [XmlIgnore()]
        public Expression LeftExpression
        {
            get
            {
                return Left.Builder.Build(_parameters);
            }
        }

        [XmlIgnore()]
        public Expression RightExpression
        {
            get
            {
                return Right.Builder.Build(_parameters);
            }
        }

        public override Expression Build()
        {
            Expression result = Expression.MakeBinary(ComparsionType, LeftExpression, RightExpression);
            return result;
        }
    }
}
