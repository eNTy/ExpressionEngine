using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace ExpressionEngine.Builders
{
    [XmlType("Constant")]
    public class ConstantBuilder : ExpressionBuilder
    {
        private string _stringValue;

        [XmlAttribute("value")]
        public string Value
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }
        
        public override Expression Build()
        {
            object value = null;
            switch (_typeName)
            {
                case("System.Boolean"):
                    value = bool.Parse(_stringValue);
                    break;
                case("System.Int32"):
                    value = int.Parse(_stringValue);
                    break;
                case ("System.String"):
                    value = _stringValue;
                    break;
                case ("System.Double"):
                    value = double.Parse(_stringValue);
                    break;
                case ("System.DateTime"):
                    value = DateTime.Parse(_stringValue);
                    break;                    
            }
            Expression result = Expression.Constant(value, Type);           
            return result;
        }
    }
}