namespace Silk.Data.SQL.Expressions
{
	public class DescendingExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Modifier;

		public QueryExpression Expression { get; }

		public DescendingExpression(QueryExpression expression)
		{
			Expression = expression;
		}
	}
}
