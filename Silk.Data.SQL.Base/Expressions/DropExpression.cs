namespace Silk.Data.SQL.Expressions
{
	public class DropExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public QueryExpression Expression { get; }

		public DropExpression(QueryExpression expression)
		{
			Expression = expression;
		}
	}
}
