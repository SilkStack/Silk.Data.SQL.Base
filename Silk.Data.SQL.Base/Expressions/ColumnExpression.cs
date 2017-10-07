namespace Silk.Data.SQL.Expressions
{
	public class ColumnExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.SchemaComponent;

		public QueryExpression Source { get; }
		public string ColumnName { get; }

		public ColumnExpression(QueryExpression source, string columnName)
		{
			Source = source;
			ColumnName = columnName;
		}
	}
}
