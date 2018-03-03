using System.Data.Common;
using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;
using System;

namespace Silk.Data.SQL.Providers
{
	public abstract class DataProviderCommonBase : QueryExecutorBase, IDataProvider
	{
		protected abstract IQueryConverter CreateQueryConverter();

		protected override SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression)
		{
			return CreateQueryConverter()
				.ConvertToQuery(queryExpression);
		}

		protected override DbCommand CreateCommand(DbConnection connection, SqlQuery sqlQuery)
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
