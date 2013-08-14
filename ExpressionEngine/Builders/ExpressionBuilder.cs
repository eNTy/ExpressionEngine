using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

namespace ExpressionEngine.Builders
{
    public abstract class ExpressionBuilder 
    {
        protected List<ParameterBuilder> _parameters;

        protected string _name;
        protected string _typeName;
        protected Type _type;

        [XmlAttribute("name")]
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlAttribute("type")]
        public virtual string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                Type type = Type.GetType(_typeName);
                if (type == null)
                {
                    throw new ArgumentException("Unknown type " + _typeName, "type");
                }
                _type = type;
            }
        }

        [XmlIgnore()]
        public virtual Type Type
        {
            get { return _type; }
        }

        public virtual Expression Build(List<ParameterBuilder> parameters)
        {
            _parameters = parameters;
            return this.Build();
        }

        public abstract Expression Build();
    }
}