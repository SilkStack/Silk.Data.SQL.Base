using System.Collections.Generic;
using System.Linq;

namespace Silk.Data.SQL.Expressions
{
	public abstract class QueryExpression
	{
		private static SqlBaseType[] _nullableTypes = new[]
		{
			SqlBaseType.Binary,
			SqlBaseType.Text
		};

		/// <summary>
		/// Gets the node type of the query expression.
		/// </summary>
		public abstract ExpressionNodeType NodeType { get; }

		public static DescendingExpression Descending(QueryExpression expression)
		{
			return new DescendingExpression(expression);
		}

		public static ColumnDefinitionExpression DefineColumn(string columnName, SqlDataType dataType,
			bool? isNullable = null, bool isAutoIncrement = false, bool isPrimaryKey = false)
		{
			if (isNullable == null)
				isNullable = _nullableTypes.Contains(dataType.BaseType);
			if (isPrimaryKey)
				isNullable = false;
			return new ColumnDefinitionExpression(columnName, dataType, isNullable.Value, isAutoIncrement, isPrimaryKey);
		}

		public static CreateTableExpression CreateTable(string tableName, IEnumerable<ColumnDefinitionExpression> columnDefinitions)
		{
			return new CreateTableExpression(tableName, columnDefinitions.ToArray());
		}

		public static CreateTableExpression CreateTable(string tableName, params ColumnDefinitionExpression[] columnDefinitions)
		{
			return CreateTable(tableName, (IEnumerable<ColumnDefinitionExpression>)columnDefinitions);
		}

		public static CreateTableIndexExpression CreateIndex(string tableName, params string[] columns)
		{
			return CreateIndex(Table(tableName), columns);
		}

		public static CreateTableIndexExpression CreateIndex(TableExpression table, params string[] columns)
		{
			return CreateIndex(table, columns.Select(q => Column(q)).ToArray());
		}

		public static CreateTableIndexExpression CreateIndex(TableExpression table, params ColumnExpression[] columns)
		{
			return CreateIndex(table, false, columns);
		}

		public static CreateTableIndexExpression CreateIndex(string tableName, bool uniqueConstraint = false, params string[] columns)
		{
			return CreateIndex(Table(tableName), uniqueConstraint, columns);
		}

		public static CreateTableIndexExpression CreateIndex(TableExpression table, bool uniqueConstraint = false, params string[] columns)
		{
			return CreateIndex(table, uniqueConstraint, columns.Select(q => Column(q)).ToArray());
		}

		public static CreateTableIndexExpression CreateIndex(TableExpression table, bool uniqueConstraint = false, params ColumnExpression[] columns)
		{
			return new CreateTableIndexExpression(table, columns, uniqueConstraint);
		}

		public static DropExpression DropTable(string tableName)
		{
			return new DropExpression(Table(tableName));
		}

		public static AssignColumnExpression Assign(string columnName, object value)
		{
			return Assign(columnName, Value(value));
		}

		public static AssignColumnExpression Assign(string columnName, QueryExpression expression)
		{
			return Assign(Column(columnName), expression);
		}

		public static AssignColumnExpression Assign(ColumnExpression column, QueryExpression expression)
		{
			return new AssignColumnExpression(column, expression);
		}

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

		public static InFunctionExpression InFunction(object[] values)
		{
			return InFunction(values.Select(value => new ValueExpression(value)).ToArray());
		}

		public static InFunctionExpression InFunction(params QueryExpression[] expressions)
		{
			return new InFunctionExpression(expressions);
		}

		public static LastInsertIdFunctionExpression LastInsertIdFunction()
		{
			return new LastInsertIdFunctionExpression();
		}

		public static CountFunctionExpression CountFunction(QueryExpression expression = null)
		{
			return new CountFunctionExpression(expression);
		}

		public static DistinctFunctionExpression Distinct(QueryExpression expression)
		{
			return new DistinctFunctionExpression(expression);
		}

		public static RandomFunctionExpression Random()
		{
			return new RandomFunctionExpression();
		}

		public static TableExistsVirtualFunctionExpression TableExists(string tableName)
		{
			return TableExists(Table(tableName));
		}

		public static TableExistsVirtualFunctionExpression TableExists(TableExpression table)
		{
			return new TableExistsVirtualFunctionExpression(table);
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

		public static QueryExpression CombineConditions(QueryExpression existing, ConditionType conditionType, QueryExpression @new)
		{
			if (existing == null)
				return @new;
			if (conditionType == ConditionType.AndAlso)
				return QueryExpression.AndAlso(existing, @new);
			return QueryExpression.OrElse(existing, @new);
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
			JoinDirection direction = JoinDirection.Inner, QueryExpression onCondition = null)
		{
			return new JoinExpression(leftColumn, rightColumn, direction, onCondition);
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
			return new InsertExpression(
				Table(tableName),
				columnNames.Select(name => Column(name)).ToArray(),
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
			return new InsertExpression(
				Table(tableName),
				columnNames.Select(name => Column(name)).ToArray(),
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
			QueryExpression where = null,
			params AssignColumnExpression[] assignments)
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
			QueryExpression whereConditions = null)
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
