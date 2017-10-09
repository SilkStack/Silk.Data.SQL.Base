namespace Silk.Data.SQL.Expressions
{
	public class UpdateExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; }
		public QueryExpression[] Assignments { get; }
		public ComparisonExpression WhereConditions { get; }

		public UpdateExpression(TableExpression table, QueryExpression[] assignments,
			ComparisonExpression whereConditions)
		{
			Table = table;
			Assignments = assignments;
			WhereConditions = whereConditions;
		}
	}
}
