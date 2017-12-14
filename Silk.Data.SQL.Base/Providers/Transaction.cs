using System.Data.Common;
using System.Threading.Tasks;
using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;

namespace Silk.Data.SQL.Providers
{
	public class Transaction : QueryExecutorBase
	{
		private readonly IDataProvider _dataProvider;
		private readonly IDataCommandCreator _dataCommandCreateor;
		private readonly DbTransaction _dbTransaction;

		public override string ProviderName => _dataProvider.ProviderName;
		public bool HasOutstandingCommits { get; private set; }

		public Transaction(IDataProvider dataProvider, IDataCommandCreator dataCommandCreator, DbTransaction dbTransaction)
		{
			_dataProvider = dataProvider;
			_dataCommandCreateor = dataCommandCreator;
			_dbTransaction = dbTransaction;
		}

		public void Commit()
		{
			_dbTransaction.Commit();
			HasOutstandingCommits = false;
		}

		public void Rollback()
		{
			_dbTransaction.Rollback();
			HasOutstandingCommits = false;
		}

		public override void Dispose()
		{
			if (HasOutstandingCommits)
				Commit();
		}

		protected override void EnsureOpen()
		{
		}

		protected override Task EnsureOpenAsync()
		{
			return Task.FromResult(true);
		}

		protected override DbCommand CreateCommand(SqlQuery sqlQuery)
		{
			var command = _dataCommandCreateor.CreateCommand(sqlQuery);
			command.Transaction = _dbTransaction;
			return command;
		}

		protected override SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression)
		{
			return _dataCommandCreateor.ConvertExpressionToQuery(queryExpression);
		}
	}
}
