using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;
using System.Data.Common;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Providers
{
	public abstract class QueryExecutorBase : IQueryProvider
	{
		protected abstract void EnsureOpen();
		protected abstract Task EnsureOpenAsync();
		protected abstract DbCommand CreateCommand(SqlQuery sqlQuery);
		protected abstract SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression);

		public abstract string ProviderName { get; }
		public abstract void Dispose();

		protected virtual int ExecuteNonQuery(SqlQuery sqlQuery)
		{
			EnsureOpen();
			using (var command = CreateCommand(sqlQuery))
			{
				return command.ExecuteNonQuery();
			}
		}

		protected virtual async Task<int> ExecuteNonQueryAsync(SqlQuery sqlQuery)
		{
			await EnsureOpenAsync()
				.ConfigureAwait(false);
			using (var command = CreateCommand(sqlQuery))
			{
				return await command.ExecuteNonQueryAsync()
					.ConfigureAwait(false);
			}
		}

		protected virtual QueryResult ExecuteReader(SqlQuery sqlQuery)
		{
			EnsureOpen();
			var command = CreateCommand(sqlQuery);
			return new QueryResult(command, command.ExecuteReader());
		}

		protected virtual async Task<QueryResult> ExecuteReaderAsync(SqlQuery sqlQuery)
		{
			await EnsureOpenAsync()
				.ConfigureAwait(false);
			var command = CreateCommand(sqlQuery);
			return new QueryResult(command, await command.ExecuteReaderAsync()
				.ConfigureAwait(false));
		}

		public virtual int ExecuteNonQuery(QueryExpression queryExpression)
		{
			return ExecuteNonQuery(ConvertExpressionToQuery(queryExpression));
		}

		public virtual Task<int> ExecuteNonQueryAsync(QueryExpression queryExpression)
		{
			return ExecuteNonQueryAsync(ConvertExpressionToQuery(queryExpression));
		}

		public virtual QueryResult ExecuteReader(QueryExpression queryExpression)
		{
			return ExecuteReader(ConvertExpressionToQuery(queryExpression));
		}

		public virtual Task<QueryResult> ExecuteReaderAsync(QueryExpression queryExpression)
		{
			return ExecuteReaderAsync(ConvertExpressionToQuery(queryExpression));
		}
	}
}
