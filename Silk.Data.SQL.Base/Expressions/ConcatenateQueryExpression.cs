namespace Silk.Data.SQL.Expressions
{
	public class ConcatenateQueryExpression : DbFunctionQueryExpression
	{
		public QueryExpression[] Expressions { get; }

		public ConcatenateQueryExpression(params QueryExpression[] expressions)
		{
			Expressions = expressions;
		}
	}
}
