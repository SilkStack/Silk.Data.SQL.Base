using System.Data.Common;
using System.Threading.Tasks;
using Silk.Data.SQL.Expressions;
using System.Data;
using Silk.Data.SQL.Queries;
using System;

namespace Silk.Data.SQL.Providers
{
	public abstract class DataProviderCommonBase : QueryExecutorBase, IDataProvider, IDataCommandCreator
	{
		protected abstract DbConnection DbConnection { get; }

		protected abstract IQueryConverter CreateQueryConverter();

		protected override Task EnsureOpenAsync()
		{
			if (DbConnection.State != ConnectionState.Open)
				return DbConnection.OpenAsync();
			return Task.FromResult(true);
		}

		protected override void EnsureOpen()
		{
			if (DbConnection.State != ConnectionState.Open)
				DbConnection.Open();
		}

		protected override SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression)
		{
			return CreateQueryConverter()
				.ConvertToQuery(queryExpression);
		}

		protected override DbCommand CreateCommand(SqlQuery sqlQuery)
		{
			var command = DbConnection.CreateCommand();
			command.CommandText = sqlQuery.SqlText;
			if (sqlQuery.QueryParameters != null)
			{
				foreach (var kvp in sqlQuery.QueryParameters)
				{
					var parameter = command.CreateParameter();
					parameter.ParameterName = $"@{kvp.Key}";
					if (kvp.Value.Value == null)
						parameter.Value = DBNull.Value;
					else
						parameter.Value = kvp.Value.Value;
					command.Parameters.Add(parameter);
				}
			}
			return command;
		}

		DbCommand IDataCommandCreator.CreateCommand(SqlQuery sqlQuery)
		{
			return CreateCommand(sqlQuery);
		}

		public override void Dispose()
		{
			if (DbConnection != null)
			{
				DbConnection.Dispose();
			}
		}

		public Transaction CreateTransaction()
		{
			EnsureOpen();
			var dbTransaction = DbConnection.BeginTransaction();
			return new Transaction(this, this, dbTransaction);
		}

		public async Task<Transaction> CreateTransactionAsync()
		{
			await EnsureOpenAsync().ConfigureAwait(false);
			var dbTransaction = DbConnection.BeginTransaction();
			return new Transaction(this, this, dbTransaction);
		}

		SqlQuery IDataCommandCreator.ConvertExpressionToQuery(QueryExpression queryExpression)
		{
			return ConvertExpressionToQuery(queryExpression);
		}
	}
}
