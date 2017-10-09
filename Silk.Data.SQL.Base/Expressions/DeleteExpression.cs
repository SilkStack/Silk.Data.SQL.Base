namespace Silk.Data.SQL.Expressions
{
	public class DeleteExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; }
		public ComparisonExpression WhereConditions { get; }

		public DeleteExpression(TableExpression table, ComparisonExpression whereConditions)
		{
			Table = table;
			WhereConditions = whereConditions;
		}
	}
}
