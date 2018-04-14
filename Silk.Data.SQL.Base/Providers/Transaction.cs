using System.Data.Common;
using System.Threading.Tasks;
using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;

namespace Silk.Data.SQL.Providers
{
	internal class Transaction : ITransaction
	{
		private readonly DbConnection _connection;
		private readonly DbTransaction _transaction;
		private readonly QueryExecutorBase _queryExecutorBase;

		public string ProviderName => _queryExecutorBase.ProviderName;

		public Transaction(DbConnection dbConnection, DbTransaction transaction, QueryExecutorBase queryExecutorBase)
		{
			_connection = dbConnection;
			_transaction = transaction;
			_queryExecutorBase = queryExecutorBase;
		}

		public int ExecuteNonQuery(QueryExpression queryExpression)
		{
			using (var command = _queryExecutorBase.CreateCommand(_connection,
				_queryExecutorBase.ConvertExpressionToQuery(queryExpression)))
			{
				command.Transaction = _transaction;
				return command.ExecuteNonQuery();
			}
		}

		public Task<int> ExecuteNonQueryAsync(QueryExpression queryExpression)
		{
			using (var command = _queryExecutorBase.CreateCommand(_connection,
				_queryExecutorBase.ConvertExpressionToQuery(queryExpression)))
			{
				command.Transaction = _transaction;
				return command.ExecuteNonQueryAsync();
			}
		}

		public QueryResult ExecuteReader(QueryExpression queryExpression)
		{
			var command = _queryExecutorBase.CreateCommand(_connection,
				_queryExecutorBase.ConvertExpressionToQuery(queryExpression));
			command.Transaction = _transaction;
			return new QueryResult(command, command.ExecuteReader(), null);
		}

		public async Task<QueryResult> ExecuteReaderAsync(QueryExpression queryExpression)
		{
			var command = _queryExecutorBase.CreateCommand(_connection,
				_queryExecutorBase.ConvertExpressionToQuery(queryExpression));
			command.Transaction = _transaction;
			return new QueryResult(command, await command.ExecuteReaderAsync(), null);
		}

		public void Commit()
		{
			_transaction.Commit();
		}

		public void Rollback()
		{
			_transaction.Rollback();
		}

		public void Dispose()
		{
			_transaction.Dispose();
			_connection.Dispose();
		}
	}
}
