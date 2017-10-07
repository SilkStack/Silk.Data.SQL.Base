namespace Silk.Data.SQL.Expressions
{
	public class TableExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.SchemaComponent;

		public string TableName { get; }

		public TableExpression(string tableName)
		{
			TableName = tableName;
		}
	}
}
