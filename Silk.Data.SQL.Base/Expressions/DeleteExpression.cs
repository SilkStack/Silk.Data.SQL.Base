namespace Silk.Data.SQL.Expressions
{
	public class DeleteExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; }
		public QueryExpression WhereConditions { get; }
		public QueryExpression Limit { get; }

		public DeleteExpression(TableExpression table, QueryExpression whereConditions,
			QueryExpression limit = null)
		{
			Table = table;
			WhereConditions = whereConditions;
			Limit = limit;
		}
	}
}
