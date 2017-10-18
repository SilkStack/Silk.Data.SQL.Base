namespace Silk.Data.SQL.Expressions
{
	public class InFunctionExpression : DbFunctionQueryExpression
	{
		public QueryExpression[] Expressions { get; }

		public InFunctionExpression(QueryExpression[] expressions)
		{
			Expressions = expressions;
		}
	}
}
