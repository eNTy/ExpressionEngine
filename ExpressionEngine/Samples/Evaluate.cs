using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using DataLayer;
using ExpressionEngine;
using System.Data;
using System.Data;

namespace Security
{
    public class Evaluate
    {       

        public Dictionary<BusinessObject, List<int>> EvaluateAll(Employee user)
        {
            Dictionary<BusinessObject, List<int>> typeAccess = new Dictionary<BusinessObject, List<int>>();

            BusinessObject objectDefinition = new BusinessObject();
            DBInteractionBaseCollection<BusinessObject> definitions = objectDefinition.SelectAll().ConvertToCollection<BusinessObject>();

            foreach (BusinessObject definition in definitions)
            {
                if (definition.RowLevelSecurity.IsNull) continue; // skip objects without row level security
                List<int> hasAccess = EvaluateCollection(definition, user);
                typeAccess.Add(definition, hasAccess);
            }

            return typeAccess;
        }

        public static List<int> EvaluateCollection(BusinessObject objectDefinition, Employee user)
        {
            // skip for no row level security
            if (objectDefinition.RowLevelSecurity.IsNull) return null;

            // get Expressions from definition
            ExpressionCollection rowLevelUsed = ExpressionFactory.FromXml(objectDefinition.RowLevelSecurity.Value);
            ExpressionCollection rowLevelEvaluation = ExpressionFactory.FromXml(objectDefinition.RowLevelSecurityEval.Value);       

            EObjectBase eObject = objectDefinition.GetNewInstance();
            //DBInteractionBaseCollection<DBInteractionBase> objects = eObject.SelectAll_RowSecurityFields().ConvertToCollection(objectDefinition.Type) as DBInteractionBaseCollection<DBInteractionBase>;
            return EvaluateCollection(eObject, user, rowLevelUsed, rowLevelEvaluation);            
        }

        public static List<int> EvaluateCollection(DBInteractionBaseCollection<DBInteractionBase> eObjects, Employee user, ExpressionCollection rowLevelUsed, ExpressionCollection rowLevelEvaluation)
        {
            // skip for empty collection
            if (eObjects == null || eObjects.Count < 1) return null;

            List<int> hasAccessIds = new List<int>();

            foreach (eObjectBase eObject in eObjects)
            {
                // first, we only check if row level permission is used at all
                // create parameters
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("eObject", eObject);

                // if an object falls in any of the definitions, it must be succefully evaluated
                bool hasAccess = true;
                foreach (string expressionName in rowLevelUsed.Expressions.Keys)
                {
                    ExpressionTree rowLevelUsedExpression = rowLevelUsed.Expressions[expressionName];
                    if (!rowLevelUsedExpression.Evaluate<bool>(parameters)) continue; // check next condition
                    
                    // row level needs to be evaluated, add current user
                    parameters.Add("eUser", user);

                    // check for existing evaluation rule first
                    if (!rowLevelEvaluation.Expressions.ContainsKey(expressionName))
                    {
                        throw new Exception("Row level evaluation rule is missing! Expected expression: " + expressionName);
                    }

                    // do evaluation, we must pass all expressions, hence the AND
                    ExpressionTree rowLevelEvalExpression = rowLevelEvaluation.Expressions[expressionName];
                    hasAccess = hasAccess & rowLevelEvalExpression.Evaluate<bool>(parameters);             
                }
                
                if (hasAccess)
                {
                    hasAccessIds.Add(eObject.IdPrimaryKey.Value);
                }
            }

            return hasAccessIds;
        }


        public static List<int> EvaluateCollection(eObjectBase businessObject, Employee user, ExpressionCollection rowLevelUsed, ExpressionCollection rowLevelEvaluation)
        {
            // skip for empty collection
            DataTable eObjects = businessObject.SelectAll_RowSecurityFields();

            if (eObjects == null || eObjects.Rows.Count < 1) return null;

            List<int> hasAccessIds = new List<int>();

            foreach (DataRow eObject in eObjects.Rows)
            {
                // first, we only check if row level permission is used at all
                // create parameters
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("eObject", eObject);

                // if an object falls in any of the definitions, it must be succefully evaluated
                bool hasAccess = true;
                foreach (string expressionName in rowLevelUsed.Expressions.Keys)
                {
                    ExpressionTree rowLevelUsedExpression = rowLevelUsed.Expressions[expressionName];
                    if (!rowLevelUsedExpression.Evaluate<bool>(parameters)) continue; // check next condition

                    // row level needs to be evaluated, add current user
                    parameters.Add("eUser", user);

                    // check for existing evaluation rule first
                    if (!rowLevelEvaluation.Expressions.ContainsKey(expressionName))
                    {
                        throw new Exception("Row level evaluation rule is missing! Expected expression: " + expressionName);
                    }

                    // do evaluation, we must pass all expressions, hence the AND
                    ExpressionTree rowLevelEvalExpression = rowLevelEvaluation.Expressions[expressionName];
                    hasAccess = hasAccess & rowLevelEvalExpression.Evaluate<bool>(parameters);
                }

                if (hasAccess)
                {
                    hasAccessIds.Add(eObject.Field<int>(businessObject.PrimaryKeyName));
                }
            }

            return hasAccessIds;
        }
    }
}