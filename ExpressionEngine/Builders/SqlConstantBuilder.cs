using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;
using System.Data.SqlTypes;

namespace ExpressionEngine.Builders
{
    [XmlType("SqlConstant")]
    public class SqlConstantBuilder : ExpressionBuilder
    {
        private string _stringValue;

        [XmlAttribute("value")]
        public string Value
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }

        [XmlAttribute("type")]
        public override string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                string fullTypeName = string.Format("System.Data.SqlTypes.{0}, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", _typeName);
                Type type = Type.GetType(fullTypeName);
                if (type == null)
                {
                    throw new ArgumentException("Unknown type " + _typeName, "type");
                }
                _type = type;
            }
        }        
        
        public override Expression Build()
        {
            object value = null;
            switch (_typeName)
            {
                case ("SqlBoolean"):
                    value = SqlBoolean.Parse(_stringValue);
                    break;
                case("SqlInt32"):
                    value = SqlInt32.Parse(_stringValue);
                    break;
                case ("SqlString"):
                    value = new SqlString(_stringValue);
                    break;
                case ("SqlDouble"):
                    value = SqlDouble.Parse(_stringValue);
                    break;
                case ("SqlDateTime"):
                    value = SqlDateTime.Parse(_stringValue);
                    break;                    
            }
            Expression result = Expression.Constant(value, typeof(object));           
            return result;
        }
    }
}