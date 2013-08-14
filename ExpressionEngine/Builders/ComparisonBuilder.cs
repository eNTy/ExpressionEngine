using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    public abstract class ComparisonBuilder : ExpressionBuilder
    {
        protected ExpressionType _comparsionType;

        [XmlAttribute("comparison")]
        public ExpressionType ComparsionType
        {
            get { return _comparsionType; }
            set { _comparsionType = value; }
        }
    }
}
