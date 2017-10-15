using System.Data.Common;
using System.Threading.Tasks;
using Silk.Data.SQL.Expressions;
using System.Data;
using Silk.Data.SQL.Queries;

namespace Silk.Data.SQL.Providers
{
	public abstract class DataProviderCommonBase : IDataProvider
	{
		public abstract string ProviderName { get; }

		protected abstract DbConnection DbConnection { get; }

		protected abstract IQueryConverter CreateQueryConverter();

		protected virtual Task EnsureOpenAsync()
		{
			if (DbConnection.State != ConnectionState.Open)
				return DbConnection.OpenAsync();
			return Task.FromResult(true);
		}

		protected virtual void EnsureOpen()
		{
			if (DbConnection.State != ConnectionState.Open)
				DbConnection.Open();
		}

		protected virtual SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression)
		{
			return CreateQueryConverter()
				.ConvertToQuery(queryExpression);
		}

		protected virtual DbCommand CreateCommand(SqlQuery sqlQuery)
		{
			var command = DbConnection.CreateCommand();
			command.CommandText = sqlQuery.SqlText;
			if (sqlQuery.QueryParameters != null)
			{
				foreach (var kvp in sqlQuery.QueryParameters)
				{
					var parameter = command.CreateParameter();
					parameter.ParameterName = $"@{kvp.Key}";
					parameter.Value = kvp.Value.Value;
					command.Parameters.Add(parameter);
				}
			}
			return command;
		}

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

		public int ExecuteNonQuery(QueryExpression queryExpression)
		{
			return ExecuteNonQuery(ConvertExpressionToQuery(queryExpression));
		}

		public Task<int> ExecuteNonQueryAsync(QueryExpression queryExpression)
		{
			return ExecuteNonQueryAsync(ConvertExpressionToQuery(queryExpression));
		}

		public virtual void Dispose()
		{
			if (DbConnection != null)
			{
				DbConnection.Dispose();
			}
		}

		public QueryResult ExecuteReader(QueryExpression queryExpression)
		{
			return ExecuteReader(ConvertExpressionToQuery(queryExpression));
		}

		public Task<QueryResult> ExecuteReaderAsync(QueryExpression queryExpression)
		{
			return ExecuteReaderAsync(ConvertExpressionToQuery(queryExpression));
		}
	}
}
