namespace Silk.Data.SQL.Expressions
{
	public class CreateTableExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public string TableName { get; }
		public ColumnDefinitionExpression[] ColumnDefinitions { get; }

		public CreateTableExpression(string tableName, ColumnDefinitionExpression[] columnDefinitions)
		{
			TableName = tableName;
			ColumnDefinitions = columnDefinitions;
		}
	}
}
