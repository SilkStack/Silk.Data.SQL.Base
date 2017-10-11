namespace Silk.Data.SQL.Expressions
{
	public class DistinctFunctionExpression : DbFunctionQueryExpression
	{
		public QueryExpression Expression { get; }

		public DistinctFunctionExpression(QueryExpression expression)
		{
			Expression = expression;
		}
	}
}
