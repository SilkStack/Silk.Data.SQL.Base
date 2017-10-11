using System.Collections.Generic;
using System.Linq;

namespace Silk.Data.SQL.Expressions
{
	public abstract class QueryExpression
	{
		/// <summary>
		/// Gets the node type of the query expression.
		/// </summary>
		public abstract ExpressionNodeType NodeType { get; }

		public static ColumnExpression All(QueryExpression source = null)
		{
			return Column("*", source);
		}

		public static TableExpression Table(string tableName)
		{
			return new TableExpression(tableName);
		}

		public static ColumnExpression Column(string columnName, QueryExpression source = null)
		{
			return new ColumnExpression(source, columnName);
		}

		public static ValueExpression Value(object value)
		{
			return new ValueExpression(value);
		}

		public static CountFunctionExpression CountFunction(QueryExpression expression = null)
		{
			return new CountFunctionExpression(expression);
		}

		public static DistinctFunctionExpression Distinct(QueryExpression expression)
		{
			return new DistinctFunctionExpression(expression);
		}

		public static RandomFunctionExpression Random(QueryExpression expression = null)
		{
			return new RandomFunctionExpression(expression);
		}

		public static AliasExpression Alias(QueryExpression expression, string alias)
		{
			return new AliasExpression(expression, alias);
		}

		public static ComparisonExpression Compare(QueryExpression left, ComparisonOperator comparisonType,
			QueryExpression right)
		{
			return new ComparisonExpression(left, comparisonType, right);
		}

		public static ConditionExpression AndAlso(QueryExpression left, QueryExpression right)
		{
			return new ConditionExpression(left, ConditionType.AndAlso, right);
		}

		public static ConditionExpression OrElse(QueryExpression left, QueryExpression right)
		{
			return new ConditionExpression(left, ConditionType.OrElse, right);
		}

		public static JoinExpression Join(ColumnExpression leftColumn, ColumnExpression rightColumn,
			JoinDirection direction = JoinDirection.Inner)
		{
			return new JoinExpression(leftColumn, rightColumn, direction);
		}

		/// <summary>
		/// Creates a query expression representing a SELECT query.
		/// </summary>
		/// <returns></returns>
		public static SelectExpression Select(
			QueryExpression[] projection,
			QueryExpression from = null,
			JoinExpression[] joins = null,
			QueryExpression where = null,
			QueryExpression having = null,
			QueryExpression[] orderBy = null,
			QueryExpression[] groupBy = null,
			QueryExpression offset = null,
			QueryExpression limit = null
			)
		{
			return new SelectExpression(projection, from, joins,
				where, having, orderBy,
				groupBy, offset, limit);
		}

		/// <summary>
		/// Creates a query expression representing an INSERT query.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="columns"></param>
		/// <param name="rowsValues"></param>
		/// <returns></returns>
		public static InsertExpression Insert(string tableName, string[] columnNames,
			params object[][] rowsValues)
		{
			var table = Table(tableName);
			return new InsertExpression(
				table,
				columnNames.Select(name => Column(name, table)).ToArray(),
				rowsValues.Select(values => values.Select(value => Value(value)).ToArray()).ToArray()
				);
		}

		/// <summary>
		/// Creates a query expression representing an INSERT query.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="columns"></param>
		/// <param name="rowsExpressions"></param>
		/// <returns></returns>
		public static InsertExpression Insert(string tableName, string[] columnNames,
			params QueryExpression[][] rowsExpressions)
		{
			var table = Table(tableName);
			return new InsertExpression(
				table,
				columnNames.Select(name => Column(name, table)).ToArray(),
				rowsExpressions
				);
		}

		/// <summary>
		/// Creates a query expression representing an INSERT query.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="columns"></param>
		/// <param name="rowsValues"></param>
		/// <returns></returns>
		public static InsertExpression Insert(TableExpression table, ColumnExpression[] columns,
			params object[][] rowsValues)
		{
			return new InsertExpression(table, columns,
				rowsValues.Select(values => values.Select(value => new ValueExpression(value)).ToArray()).ToArray()
				);
		}

		/// <summary>
		/// Creates a query expression representing an INSERT query.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="columnNames"></param>
		/// <param name="rowsExpressions"></param>
		/// <returns></returns>
		public static InsertExpression Insert(TableExpression table, string[] columnNames,
			params QueryExpression[][] rowsExpressions)
		{
			return new InsertExpression(table,
				columnNames.Select(name => Column(name, table)).ToArray(),
				rowsExpressions);
		}

		/// <summary>
		/// Creates a query expression representing an UPDATE query.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="where"></param>
		/// <param name="assignments"></param>
		/// <returns></returns>
		public static UpdateExpression Update(TableExpression table,
			ComparisonExpression where = null,
			params QueryExpression[] assignments)
		{
			return new UpdateExpression(table, assignments, where);
		}

		/// <summary>
		/// Creates a query expression representing a DELETE query.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="whereConditions"></param>
		/// <returns></returns>
		public static DeleteExpression Delete(TableExpression table,
			ComparisonExpression whereConditions = null)
		{
			return new DeleteExpression(table, whereConditions);
		}

		/// <summary>
		/// Creates a query expression repsenting execution of a stored procedure.
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public static ExecuteStoredProcedureExpression ExecuteStoredProcedure(string storedProcedureName,
			params QueryExpression[] arguments)
		{
			return new ExecuteStoredProcedureExpression(storedProcedureName, arguments);
		}

		/// <summary>
		/// Creates a query expression representing a transaction query.
		/// </summary>
		/// <remarks>Nested transaction queries are not supported.</remarks>
		/// <param name="expressions"></param>
		/// <returns></returns>
		public static TransactionExpression Transaction(params QueryExpression[] expressions)
		{
			return Transaction((IEnumerable<QueryExpression>)expressions);
		}

		/// <summary>
		/// Creates a query expression representing a transaction query.
		/// </summary>
		/// <remarks>Nested transaction queries are not supported.</remarks>
		/// <param name="expressions"></param>
		/// <returns></returns>
		public static TransactionExpression Transaction(IEnumerable<QueryExpression> expressions)
		{
			return new TransactionExpression(expressions);
		}
	}
}
