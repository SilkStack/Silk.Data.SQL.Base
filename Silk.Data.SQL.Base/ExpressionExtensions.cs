using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL
{
	public static class ExpressionExtensions
	{
		public static SelectExpression ToCountQuery(this SelectExpression selectExpression)
		{
			return new SelectExpression(
				new[] { QueryExpression.CountFunction(QueryExpression.All()) },
				selectExpression.From, selectExpression.Joins,
				selectExpression.WhereConditions, selectExpression.HavingConditions,
				null, selectExpression.GroupConditions, null, null
				);
		}
	}
}
