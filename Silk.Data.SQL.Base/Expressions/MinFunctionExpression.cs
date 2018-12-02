namespace Silk.Data.SQL.Expressions
{
	public class MinFunctionExpression : DbFunctionQueryExpression
	{
		public QueryExpression Expression { get; }

		public MinFunctionExpression(QueryExpression expression = null)
		{
			Expression = expression;
		}
	}
}
