using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using ExpressionEngine.Builders;

namespace ExpressionEngine
{
    public class ExpressionSide
    {
        protected ExpressionBuilder _builder;
        
        public ExpressionBuilder Builder
        {
            get { return _builder; }
            set { _builder = value; }
        }
    }
}