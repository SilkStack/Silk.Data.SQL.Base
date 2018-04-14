using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;
using System.Data.Common;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Providers
{
	public abstract class QueryExecutorBase : IQueryProvider
	{
		protected abstract DbConnection Connect();
		protected abstract Task<DbConnection> ConnectAsync();
		public abstract DbCommand CreateCommand(DbConnection connection, SqlQuery sqlQuery);
		public abstract SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression);

		public abstract string ProviderName { get; }
		public abstract void Dispose();

		protected virtual int ExecuteNonQuery(SqlQuery sqlQuery)
		{
			using (var connection = Connect())
			using (var command = CreateCommand(connection, sqlQuery))
			{
				return command.ExecuteNonQuery();
			}
		}

		protected virtual async Task<int> ExecuteNonQueryAsync(SqlQuery sqlQuery)
		{
			using (var connection = await ConnectAsync())
			using (var command = CreateCommand(connection, sqlQuery))
			{
				return await command.ExecuteNonQueryAsync()
					.ConfigureAwait(false);
			}
		}

		protected virtual QueryResult ExecuteReader(SqlQuery sqlQuery)
		{
			var connection = Connect();
			var command = CreateCommand(connection, sqlQuery);
			return new QueryResult(command, command.ExecuteReader(), connection);
		}

		protected virtual async Task<QueryResult> ExecuteReaderAsync(SqlQuery sqlQuery)
		{
			var connection = await ConnectAsync();
			var command = CreateCommand(connection, sqlQuery);
			return new QueryResult(command, await command.ExecuteReaderAsync(), connection);
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
