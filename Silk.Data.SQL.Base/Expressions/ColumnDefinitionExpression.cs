namespace Silk.Data.SQL.Expressions
{
	public class ColumnDefinitionExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.ColumnDefinition;

		public string ColumnName { get; }
		public SqlDataType DataType { get; }
		public bool IsNullable { get; }
		public bool IsAutoIncrement { get; }
		public bool IsPrimaryKey { get; }

		public ColumnDefinitionExpression(string columnName, SqlDataType dataType,
			bool isNullable, bool isAutoIncrement = false, bool isPrimaryKey = false)
		{
			ColumnName = columnName;
			DataType = dataType;
			IsNullable = isNullable;
			IsAutoIncrement = isAutoIncrement;
			IsPrimaryKey = isPrimaryKey;
		}
	}
}
