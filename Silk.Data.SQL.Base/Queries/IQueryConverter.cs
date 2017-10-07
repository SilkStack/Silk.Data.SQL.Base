using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Queries
{
	/// <summary>
	/// Converts query expressions to sql queries.
	/// </summary>
	public interface IQueryConverter
	{
		/// <summary>
		/// Converts the given expression into an SQL query.
		/// </summary>
		/// <param name="queryExpression"></param>
		/// <returns></returns>
		SqlQuery ConvertToQuery(QueryExpression queryExpression);
	}
}
