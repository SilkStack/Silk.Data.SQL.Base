namespace Silk.Data.SQL.Expressions
{
	public class DeleteExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; }
		public ConditionExpression WhereConditions { get; }

		public DeleteExpression(TableExpression table, ConditionExpression whereConditions)
		{
			Table = table;
			WhereConditions = whereConditions;
		}
	}
}
