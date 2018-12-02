namespace Silk.Data.SQL.Expressions
{
	public class MaxFunctionExpression : DbFunctionQueryExpression
	{
		public QueryExpression Expression { get; }

		public MaxFunctionExpression(QueryExpression expression = null)
		{
			Expression = expression;
		}
	}
}
