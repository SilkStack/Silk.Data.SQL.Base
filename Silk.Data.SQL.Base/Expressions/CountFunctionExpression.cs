namespace Silk.Data.SQL.Expressions
{
	public class CountFunctionExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.DbFunction;

		public QueryExpression Expression { get; }

		public CountFunctionExpression(QueryExpression expression)
		{
			Expression = expression;
		}
	}
}
