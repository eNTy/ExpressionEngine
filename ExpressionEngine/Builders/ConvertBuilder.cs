using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Convert")]
    public class ConvertBuilder : UnaryBuilder
    {
        private bool _checked;

        [XmlAttribute("checked")]
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }
        
        public override Expression Build()
        {
            if (_checked)
            {
                return Expression.ConvertChecked(OperandExpression, Type);
            }
            else
            {
                return Expression.Convert(OperandExpression, Type);               
            }
        }
    }
}