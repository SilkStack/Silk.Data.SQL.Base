using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;
using System;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Providers
{
	public interface IQueryProvider : IDisposable
	{
		/// <summary>
		/// Gets the provider's name.
		/// </summary>
		string ProviderName { get; }

		/// <summary>
		/// Execute a query and return the number of affected rows.
		/// </summary>
		/// <param name="queryExpression"></param>
		int ExecuteNonQuery(QueryExpression queryExpression);
		/// <summary>
		/// Execute a query and return the number of affected rows
		/// </summary>
		/// <param name="queryExpression"></param>
		/// <returns></returns>
		Task<int> ExecuteNonQueryAsync(QueryExpression queryExpression);

		QueryResult ExecuteReader(QueryExpression queryExpression);

		Task<QueryResult> ExecuteReaderAsync(QueryExpression queryExpression);
	}
}
