namespace Silk.Data.SQL.Expressions
{
	public class UpdateExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; }
		public AssignColumnExpression[] Assignments { get; }
		public QueryExpression WhereConditions { get; }

		public UpdateExpression(TableExpression table, AssignColumnExpression[] assignments,
			QueryExpression whereConditions)
		{
			Table = table;
			Assignments = assignments;
			WhereConditions = whereConditions;
		}
	}
}
