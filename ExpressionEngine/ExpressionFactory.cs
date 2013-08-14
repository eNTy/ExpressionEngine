using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;

using ExpressionEngine.Builders;

namespace ExpressionEngine
{
    public class ExpressionFactory
    {
        public static ExpressionCollection FromXml(string xmlExpression)
        {
            using (StringReader stringReader = new StringReader(xmlExpression))
            {
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    return FromXml(reader);
                }
            }
        }

        public static ExpressionCollection FromXml(StreamReader stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                return FromXml(reader);
            }
        }

        //public static ExpressionCollection FromXml(string filePath)
        //{
        //    using (XmlReader reader = XmlReader.Create(System.IO.File.OpenText(filePath)))
        //    {
        //        return FromXml(reader);
        //    }
        //}

        public static ExpressionCollection FromXml(XmlReader reader)
        {
            XmlAttributeOverrides overrides = GetAttributeOverrides();
            XmlSerializer serializer = new XmlSerializer(typeof(ExpressionCollection), overrides);
            return serializer.Deserialize(reader) as ExpressionCollection;
        }

        public static XmlAttributeOverrides GetAttributeOverrides()
        {
            // ExpressionBuilder is abstract class
            // create overrides for ExpressionBuilder properties
            // collect all classes inheriting from ExpressionBuilder
            XmlAttributes attributes = new XmlAttributes();

            foreach (Type type in getDerivedTypes<ExpressionBuilder>())
            {
                attributes.XmlElements.Add(getElementOverride(type));
            }

            // create overrider
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            overrides.Add(typeof(ExpressionSide), "Builder", attributes);
            overrides.Add(typeof(UnaryBuilder), "Builder", attributes);

            return overrides;
        }

        /// <summary>
        /// Get element attribute
        /// Collect custom XmlType name if available, otherwise use class Name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static XmlElementAttribute getElementOverride(Type type)
        {
            string elementName = type.Name;
            object[] attributes = type.GetCustomAttributes(typeof(XmlTypeAttribute), false);
            if (attributes.Length > 0)
            {
                XmlTypeAttribute typeAttribute = type.GetCustomAttributes(typeof(XmlTypeAttribute), false)[0] as XmlTypeAttribute;
                if (typeAttribute != null)
                {
                    elementName = typeAttribute.TypeName;
                }
            }
            XmlElementAttribute element = new XmlElementAttribute(elementName, type);
            return element;
        }

        /// <summary>
        /// Get all non-abstract classes inheriting from T from a given assembly 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static List<Type> getDerivedTypes<T>(Assembly assembly) where T : class
        {
            return assembly.GetTypes().
                    Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))).
                    ToList();
        }

        /// <summary>
        /// Get all non-abstract classes inheriting from T from all loaded assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<Type> getDerivedTypes<T>() where T : class
        {
            List<Type> derivedTypes = new List<Type>();
            AppDomain domain = AppDomain.CurrentDomain;
            Assembly[] AssembliesLoaded = domain.GetAssemblies();
            foreach (Assembly assembly in AssembliesLoaded)
            {
                derivedTypes.AddRange(getDerivedTypes<T>(assembly));
            }
            return derivedTypes;
        }
    }
}
