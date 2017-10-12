namespace Silk.Data.SQL.Expressions
{
	public class DeleteExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; }
		public QueryExpression WhereConditions { get; }

		public DeleteExpression(TableExpression table, QueryExpression whereConditions)
		{
			Table = table;
			WhereConditions = whereConditions;
		}
	}
}
