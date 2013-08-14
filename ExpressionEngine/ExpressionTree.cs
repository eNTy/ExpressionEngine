using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq.Expressions;
using System.Linq;

using ExpressionEngine.Builders;

namespace ExpressionEngine
{
    public class ExpressionTree
    {
        private string _name;
        protected ExpressionSide _expressionRoot;
        protected List<ParameterBuilder> _parameters;
        protected Delegate _evaluationMethod;
        protected LambdaExpression _evaluationLambda;

        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlArray("Parameters")]
        public List<ParameterBuilder> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        [XmlElement("Expression")]
        public ExpressionSide ExpressionRoot
        {
            get { return _expressionRoot; }
            set { _expressionRoot = value; }
        }

        [XmlIgnore()]
        public Delegate EvaluationMethod
        {
            get 
            {
                if (_evaluationMethod == null)
                {
                    BuildExpressionTree();
                }
                return _evaluationMethod;            
            }
        }

        [XmlIgnore()]
        public LambdaExpression EvaluationLambda
        {
            get 
            {
                if (_evaluationLambda == null)
                {
                    BuildExpressionTree();
                }
                return _evaluationLambda;            
            }
        }

        public override string ToString()
        {
            return EvaluationLambda.ToString();
        }       

        public virtual object Evaluate(IDictionary<string, object> parameterValues)
        {
            // the goal here is to use lambda parameters order and create
            // a list of parameters (from passed dictionary) in the same order
            // for the DynamicInvoke to work
            List<object> parameters = new List<object>();
            foreach (ParameterExpression parameter in EvaluationLambda.Parameters)
            {
                if (parameterValues.ContainsKey(parameter.Name))
                {
                    // add value in the list
                    parameters.Add(parameterValues[parameter.Name]);
                }
                else
                {
                    // add null to the list (skipped parameters)
                    parameters.Add(null);
                }
            }
            object result = EvaluationMethod.DynamicInvoke(parameters.ToArray());
            return result;
        }

        public virtual T Evaluate<T>(IDictionary<string, object> parameters)
        {
            return (T)Evaluate(parameters);
        }

        protected virtual void BuildExpressionTree()
        {
            // param list
            List<ParameterExpression> parameters = _parameters.Select(item => item.Parameter).ToList();

            // build expression tree
            Expression rootExpression = ExpressionRoot.Builder.Build(_parameters);

            //string debugCode = rootExpression.ToString();

            _evaluationLambda = Expression.Lambda(rootExpression, parameters);
            _evaluationMethod = _evaluationLambda.Compile();

            // create eval function
            //Func<EObjectBase, Employee, bool> evaluation =
            //    Expression.Lambda<Func<EObjectBase, Employee, bool>>(rootExpression, parameters).Compile();

            //bool result = evaluation(objectToCheck, currentUser);
        }

    }
}