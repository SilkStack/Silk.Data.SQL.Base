namespace Silk.Data.SQL.Expressions
{
	public class ColumnExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.SchemaComponent;

		public new TableExpression Table { get; }
		public string ColumnName { get; }

		public ColumnExpression(TableExpression table, string columnName)
		{
			Table = table;
			ColumnName = columnName;
		}
	}
}
