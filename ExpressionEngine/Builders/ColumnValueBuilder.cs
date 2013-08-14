using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Linq.Expressions;
using System.Data;

namespace ExpressionEngine.Builders
{
    /// <summary>
    /// Returns a column value from data row
    /// </summary>
    [XmlType("Column")]
    public class ColumnValueBuilder : UnaryBuilder
    {
        
        public override Expression Build()
        {
            // column name constant
            ConstantExpression columnName = Expression.Constant(Name, typeof(string));
            // field value
            IndexExpression dataColumnValue = Expression.Property(OperandExpression, "Item", columnName);

            // convert the column object value to proper data type
            Expression result = Expression.Convert(dataColumnValue, Type);

            return result;
        }
    }
}