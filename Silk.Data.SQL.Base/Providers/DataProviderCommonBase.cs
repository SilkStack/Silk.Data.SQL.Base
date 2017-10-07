﻿using System.Data.Common;
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
			return Task.CompletedTask;
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
	}
}
