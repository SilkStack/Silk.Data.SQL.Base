namespace Silk.Data.SQL.Expressions
{
	public class RandomFunctionExpression : DbFunctionQueryExpression
	{
		public QueryExpression Expression { get; }

		public RandomFunctionExpression(QueryExpression expression = null)
		{
			Expression = expression;
		}
	}
}
