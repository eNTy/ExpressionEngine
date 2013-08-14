using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;

using ExpressionEngine.Builders;

namespace ExpressionEngine
{
    public class ExpressionCollection :
        IXmlSerializable
    {
        protected Dictionary<string, ExpressionTree> _expressions;

        public Dictionary<string, ExpressionTree> Expressions
        {
            get { return _expressions; }
            set { _expressions = value; }
        }

        public ExpressionCollection()
        {
            _expressions = new Dictionary<string, ExpressionTree>();
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XDocument doc = XDocument.Load(reader);
            foreach (XNode node in doc.Descendants("ExpressionTree"))
            {
                XmlAttributeOverrides overrides = ExpressionFactory.GetAttributeOverrides();
                XmlSerializer serializer = new XmlSerializer(typeof(ExpressionTree), overrides);
                ExpressionTree tree = serializer.Deserialize(node.CreateReader()) as ExpressionTree;
                Expressions.Add(tree.Name, tree);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}