using System.Data.Common;
using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;
using System;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Providers
{
	public abstract class DataProviderCommonBase : QueryExecutorBase, IDataProvider
	{
		public virtual ITransaction CreateTransaction()
		{
			var connection = Connect();
			var transaction = connection.BeginTransaction();
			return null;
		}

		public virtual Task<ITransaction> CreateTransactionAsync()
		{
			return null;
		}

		protected abstract IQueryConverter CreateQueryConverter();

		public override SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression)
		{
			return CreateQueryConverter()
				.ConvertToQuery(queryExpression);
		}

		public override DbCommand CreateCommand(DbConnection connection, SqlQuery sqlQuery)
		{
			var command = connection.CreateCommand();
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
	}
}
