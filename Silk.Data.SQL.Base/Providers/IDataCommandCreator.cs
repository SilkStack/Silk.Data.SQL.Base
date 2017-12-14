using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;
using System.Data.Common;

namespace Silk.Data.SQL.Providers
{
	public interface IDataCommandCreator
	{
		SqlQuery ConvertExpressionToQuery(QueryExpression queryExpression);
		DbCommand CreateCommand(SqlQuery sqlQuery);
	}
}
