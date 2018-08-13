using Silk.Data.SQL.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Silk.Data.SQL.Queries
{
	public abstract class QueryConverterCommonBase : IQueryConverter
	{
		protected abstract string ProviderName { get; }
		protected abstract string AutoIncrementSql { get; }

		protected StringBuilder Sql { get; }
		protected QueryWriter ExpressionWriter { get; set; }

		public QueryConverterCommonBase()
		{
			Sql = new StringBuilder();
			ExpressionWriter = new QueryWriter(Sql, this);
		}

		public SqlQuery ConvertToQuery(QueryExpression queryExpression)
		{
			Sql.Clear();
			ExpressionWriter.Parameters = null;
			ExpressionWriter.QueryDepth = 0;
			ExpressionWriter.Visit(queryExpression);

			return new SqlQuery(ProviderName, Sql.ToString(), ExpressionWriter.Parameters);
		}

		protected virtual string GetArithmaticOperatorString(ArithmaticOperator @operator)
		{
			switch (@operator)
			{
				case ArithmaticOperator.Addition: return "+";
				case ArithmaticOperator.Division: return "/";
				case ArithmaticOperator.Multiplication: return "*";
				case ArithmaticOperator.Subtraction: return "-";
			}
			return null;
		}

		protected virtual string GetConditionOperatorString(ComparisonOperator @operator)
		{
			switch (@operator)
			{
				case ComparisonOperator.None:
					return "";
				case ComparisonOperator.AreEqual:
					return "=";
				case ComparisonOperator.AreNotEqual:
					return "!=";
				case ComparisonOperator.GreaterThan:
					return ">";
				case ComparisonOperator.GreaterThanOrEqualTo:
					return ">=";
				case ComparisonOperator.LessThan:
					return "<";
				case ComparisonOperator.LessThanOrEqualTo:
					return "<=";
				case ComparisonOperator.Like:
					return "LIKE";
			}
			return null;
		}

		protected virtual string GetBitwiseOperatorString(BitwiseOperator @operator)
		{
			switch (@operator)
			{
				case BitwiseOperator.And:
					return "&";
				case BitwiseOperator.Or:
					return "|";
				case BitwiseOperator.ExclusiveOr:
					return "^";
			}
			return null;
		}

		protected virtual void WriteFunctionToSql(QueryExpression queryExpression)
		{
			switch (queryExpression)
			{
				case TableExistsVirtualFunctionExpression tableExistsExpression:
					/*
					 * This is here to document the need to implement this "virtual" function.
					 * Providers should write a complete query that returns 1 if the table exists,
					 * non-1 or no rows if the table doesn't exist.
					 */
					break;
				//  documented functions that have no generic implementation
				case LastInsertIdFunctionExpression lastInsertIdExpression:
					break;
				case ConcatenateQueryExpression concatenateQueryExpression:
					Sql.Append(" CONCAT( ");
					for (var i = 0; i < concatenateQueryExpression.Expressions.Length; i++)
					{
						ExpressionWriter.Visit(concatenateQueryExpression.Expressions[i]);
						if (i < concatenateQueryExpression.Expressions.Length - 1)
							Sql.Append(", ");
					}
					Sql.Append(") ");
					break;
				case InFunctionExpression inExpression:
					var bracesNeeded = !(inExpression.Expressions.FirstOrDefault() is SelectExpression);
					Sql.Append(" IN ");
					if (bracesNeeded)
						Sql.Append("(");
					for (var i = 0; i < inExpression.Expressions.Length; i++)
					{
						ExpressionWriter.Visit(inExpression.Expressions[i]);
						if (i < inExpression.Expressions.Length - 1)
							Sql.Append(", ");
					}
					if (bracesNeeded)
						Sql.Append(")");
					Sql.Append(" ");
					break;
				case DistinctFunctionExpression distinctExpression:
					Sql.Append(" DISTINCT ");
					ExpressionWriter.Visit(distinctExpression.Expression);
					break;
				case CountFunctionExpression countExpression:
					Sql.Append(" COUNT(");
					if (countExpression.Expression != null)
						ExpressionWriter.Visit(countExpression.Expression);
					Sql.Append(") ");
					break;
				case RandomFunctionExpression randomExpression:
					Sql.Append(" RANDOM()");
					break;
			}
		}

		protected abstract string QuoteIdentifier(string schemaComponent);

		protected abstract string GetDbDatatype(SqlDataType sqlDataType);

		protected class QueryWriter : QueryExpressionVisitor
		{
			protected StringBuilder Sql { get; }
			public QueryConverterCommonBase Converter { get; }
			public Dictionary<string,QueryParameter> Parameters { get; set; }
			public int QueryDepth { get; set; }

			public QueryWriter(StringBuilder sql, QueryConverterCommonBase converter)
			{
				Sql = sql;
				Converter = converter;
			}

			protected override void VisitQuery(QueryExpression queryExpression)
			{
				var isSubQuery = QueryDepth > 0;
				if (isSubQuery)
				{
					Sql.Append("(");
				}

				switch (queryExpression)
				{
					case SelectExpression select:
						QueryDepth++;
						Sql.Append("SELECT ");
						VisitExpressionGroup(select.Projection, ExpressionGroupType.Projection);
						if (select.From != null)
						{
							Sql.Append(" FROM ");
							Visit(select.From);
						}
						if (select.Joins != null && select.Joins.Length > 0)
						{
							VisitExpressionGroup(select.Joins, ExpressionGroupType.Joins);
						}
						if (select.WhereConditions != null)
						{
							Sql.Append(" WHERE ");
							Visit(select.WhereConditions);
						}
						if (select.GroupConditions != null && select.GroupConditions.Length > 0)
						{
							Sql.Append(" GROUP BY ");
							VisitExpressionGroup(select.GroupConditions, ExpressionGroupType.GroupByClauses);
						}
						if (select.HavingConditions != null)
						{
							Sql.Append(" HAVING ");
							Visit(select.HavingConditions);
						}
						if (select.OrderConditions != null && select.OrderConditions.Length > 0)
						{
							Sql.Append(" ORDER BY ");
							VisitExpressionGroup(select.OrderConditions, ExpressionGroupType.OrderByClauses);
						}
						if (select.Limit != null)
						{
							Sql.Append(" LIMIT ");
							Visit(select.Limit);
						}
						if (select.Offset != null)
						{
							if (select.Limit == null)
							{
								Sql.Append(" LIMIT ");
								Visit(QueryExpression.Value(int.MaxValue));
							}
							Sql.Append(" OFFSET ");
							Visit(select.Offset);
						}
						QueryDepth--;
						break;
					case InsertExpression insert:
						QueryDepth++;
						Sql.Append("INSERT INTO ");
						Visit(insert.Table);
						Sql.Append(" (");
						VisitExpressionGroup(insert.Columns, ExpressionGroupType.ColumnList);
						Sql.Append(") ");
						Sql.Append(" VALUES ");
						for (var i = 0; i < insert.RowsExpressions.Length; i++)
						{
							Sql.Append(" (");
							VisitExpressionGroup(insert.RowsExpressions[i], ExpressionGroupType.RowValues);
							Sql.Append(") ");
							if (i < insert.RowsExpressions.Length - 1)
								Sql.Append(", ");
						}
						QueryDepth--;
						break;
					case UpdateExpression update:
						QueryDepth++;
						Sql.Append("UPDATE ");
						Visit(update.Table);
						Sql.Append(" SET ");
						VisitExpressionGroup(update.Assignments, ExpressionGroupType.RowAssignments);
						if (update.WhereConditions != null)
						{
							Sql.Append(" WHERE ");
							Visit(update.WhereConditions);
						}
						QueryDepth--;
						break;
					case DeleteExpression delete:
						QueryDepth++;
						Sql.Append("DELETE FROM ");
						Visit(delete.Table);
						if (delete.WhereConditions != null)
						{
							Sql.Append(" WHERE ");
							Visit(delete.WhereConditions);
						}
						QueryDepth--;
						break;
					case ExecuteStoredProcedureExpression sprocExec:
						QueryDepth++;
						Sql.Append($"EXECUTE {Converter.QuoteIdentifier(sprocExec.StoredProcedureName)} ");
						if (sprocExec.Arguments != null && sprocExec.Arguments.Length > 0)
						{
							VisitExpressionGroup(sprocExec.Arguments, ExpressionGroupType.ProcedureArguments);
						}
						QueryDepth--;
						break;
					case TransactionExpression transaction:
						Sql.AppendLine("BEGIN TRANSACTION;");
						foreach (var query in transaction.Queries)
						{
							Visit(query);
						}
						Sql.AppendLine("COMMIT;");
						break;
					case CreateTableExpression create:
						Sql.Append($"CREATE TABLE {Converter.QuoteIdentifier(create.TableName)} (");
						VisitExpressionGroup(create.ColumnDefinitions, ExpressionGroupType.ColumnDefinitions);
						var primaryKeyColumnNames = create.ColumnDefinitions
								.Where(q => q.IsPrimaryKey)
								.Select(q => Converter.QuoteIdentifier(q.ColumnName))
								.ToArray();
						if (primaryKeyColumnNames.Length > 0)
						{
							Sql.Append($", CONSTRAINT {Converter.QuoteIdentifier($"PK_{create.TableName}")} PRIMARY KEY ({string.Join(",", primaryKeyColumnNames)})");
						}
						Sql.Append(")");
						break;
					case CreateTableIndexExpression createIndex:
						var unique = createIndex.UniqueConstraint ? "UNIQUE" : "";
						var indexColumnNames = string.Join("_", createIndex.Columns.Select(q => q.ColumnName));
						var truncatedColumnNames = indexColumnNames;
						if (truncatedColumnNames.Length > 14)
							truncatedColumnNames = indexColumnNames.Substring(0, 14);
						var indexName = $"idx{indexColumnNames.Length}_{createIndex.Table.TableName}_{truncatedColumnNames}";
						Sql.Append($"CREATE {unique} INDEX {indexName} ON {Converter.QuoteIdentifier(createIndex.Table.TableName)} (");
						VisitExpressionGroup(createIndex.Columns, ExpressionGroupType.ColumnList);
						Sql.Append(")");
						break;
					case DropExpression drop:
						Sql.Append("DROP ");
						if (drop.Expression is TableExpression)
							Sql.Append("TABLE ");
						Visit(drop.Expression);
						break;
				}

				if (isSubQuery)
				{
					Sql.Append(") ");
				}
				else
				{
					Sql.Append("; ");
				}
			}

			protected override void VisitExpressionGroup(ICollection<QueryExpression> queryExpressions, ExpressionGroupType groupType)
			{
				switch (groupType)
				{
					case ExpressionGroupType.Queries:
						foreach (var query in queryExpressions)
						{
							Visit(query);
							Sql.AppendLine(";");
						}
						return;
					case ExpressionGroupType.RowValues:
					case ExpressionGroupType.ColumnList:
					case ExpressionGroupType.ColumnDefinitions:
					case ExpressionGroupType.GroupByClauses:
					case ExpressionGroupType.OrderByClauses:
					case ExpressionGroupType.Projection:
					case ExpressionGroupType.RowAssignments:
					case ExpressionGroupType.ProcedureArguments:
						{
							var expressionCount = queryExpressions.Count;
							var i = 0;
							foreach (var expression in queryExpressions)
							{
								i++;
								Visit(expression);
								if (i < expressionCount)
									Sql.Append(", ");
							}
						}
						return;
				}
				base.VisitExpressionGroup(queryExpressions, groupType);
			}

			protected override void VisitSchemaComponent(QueryExpression queryExpression)
			{
				switch (queryExpression)
				{
					case AliasIdentifierExpression aliasIdentifierExpression:
						Sql.Append(Converter.QuoteIdentifier(aliasIdentifierExpression.Identifier));
						break;
					case TableExpression tableExpression:
						Sql.Append(Converter.QuoteIdentifier(tableExpression.TableName));
						return;
					case ColumnExpression columnExpression:
						if (columnExpression.Source != null)
						{
							Visit(columnExpression.Source);
							Sql.Append(".");
						}
						Sql.Append(Converter.QuoteIdentifier(columnExpression.ColumnName));
						return;
				}
				base.VisitSchemaComponent(queryExpression);
			}

			protected override void VisitValue(QueryExpression queryExpression)
			{
				if (queryExpression is ValueExpression valueExpression)
				{
					if (Parameters == null)
						Parameters = new Dictionary<string, QueryParameter>();

					var parameterName = $"valueParameter{Parameters.Count + 1}";
					Sql.Append($" @{parameterName} ");
					Parameters.Add(parameterName, new QueryParameter(parameterName) { Value = valueExpression.Value });
					return;
				}
				base.VisitValue(queryExpression);
			}

			protected override void VisitAlias(QueryExpression queryExpression)
			{
				base.VisitAlias(queryExpression);

				if (queryExpression is AliasExpression aliasExpression)
				{
					Sql.Append(" AS ");
					Visit(aliasExpression.Identifier);
				}
			}

			protected override void VisitBinary(QueryExpression queryExpression)
			{
				var binaryExpression = queryExpression as BinaryQueryExpression;
				if (binaryExpression == null)
					return;

				Sql.Append("(");
				Visit(binaryExpression.Left);

				if (binaryExpression.BinaryOperationType == BinaryOperation.Comparison &&
					queryExpression is ComparisonExpression comparisonExpression)
				{
					var operatorStr = Converter.GetConditionOperatorString(comparisonExpression.Operator);
					if (operatorStr == null)
						throw new System.InvalidOperationException($"Unsupported condition operator: {comparisonExpression.Operator}");

					Sql.Append($" {operatorStr} ");
				}
				else if (binaryExpression.BinaryOperationType == BinaryOperation.Condition &&
					queryExpression is ConditionExpression conditionExpression)
				{
					if (conditionExpression.ConditionType == ConditionType.AndAlso)
						Sql.Append(" AND ");
					else
						Sql.Append(" OR ");
				}
				else if (binaryExpression.BinaryOperationType == BinaryOperation.Bitwise &&
					queryExpression is BitwiseOperationQueryExpression bitwiseExpression)
				{
					var operatorStr = Converter.GetBitwiseOperatorString(bitwiseExpression.Operator);
					if (operatorStr == null)
						throw new System.InvalidOperationException($"Unsupported condition operator: {bitwiseExpression.Operator}");

					Sql.Append($" {operatorStr} ");
				}
				else if (binaryExpression.BinaryOperationType == BinaryOperation.Arithmatic &&
					queryExpression is ArithmaticQueryExpression arithmaticExpression)
				{
					var operatorStr = Converter.GetArithmaticOperatorString(arithmaticExpression.Operator);
					if (operatorStr == null)
						throw new System.InvalidOperationException($"Unsupported arithmatic operator: {arithmaticExpression.Operator}");

					Sql.Append($" {operatorStr} ");
				}

				Visit(binaryExpression.Right);
				Sql.Append(")");
			}

			protected override void VisitFunction(QueryExpression queryExpression)
			{
				Converter.WriteFunctionToSql(queryExpression);
			}

			protected override void VisitJoin(QueryExpression queryExpression)
			{
				if (queryExpression is JoinExpression joinExpression)
				{
					switch (joinExpression.Direction)
					{
						case JoinDirection.Inner:
							Sql.Append(" INNER ");
							break;
						case JoinDirection.Left:
							Sql.Append(" LEFT OUTER ");
							break;
						case JoinDirection.Right:
							Sql.Append(" RIGHT OUTER ");
							break;
					}
					Sql.Append("JOIN ");
					Visit(joinExpression.RightColumn.Source);
					Sql.Append(" ON ");
					if (joinExpression.OnCondition == null)
					{
						VisitPossiblyAliasedColumn(joinExpression.LeftColumn);
						Sql.Append(" = ");
						VisitPossiblyAliasedColumn(joinExpression.RightColumn);
					}
					else
					{
						Visit(joinExpression.OnCondition);
					}
				}
			}

			protected override void VisitAssignment(QueryExpression queryExpression)
			{
				if (queryExpression is AssignColumnExpression assignmentExpression)
				{
					Visit(assignmentExpression.Column);
					Sql.Append(" = ");
					Visit(assignmentExpression.Expression);
				}
			}

			protected override void VisitModifier(QueryExpression queryExpression)
			{
				if (queryExpression is DescendingExpression descendingExpression)
				{
					Visit(descendingExpression.Expression);
					Sql.Append(" DESC ");
				}
			}

			protected override void VisitColumnDefinition(QueryExpression queryExpression)
			{
				if (queryExpression is ColumnDefinitionExpression columnDefinitionExpression)
				{
					Sql.Append(Converter.QuoteIdentifier(columnDefinitionExpression.ColumnName));
					Sql.Append(" ");
					if (columnDefinitionExpression.DataType.Unsigned)
					{
						Sql.Append("UNSIGNED ");
					}
					Sql.Append(Converter.GetDbDatatype(columnDefinitionExpression.DataType));
					if (!columnDefinitionExpression.IsNullable)
					{
						Sql.Append(" NOT NULL");
					}
					if (columnDefinitionExpression.IsAutoIncrement)
					{
						Sql.Append($" {Converter.AutoIncrementSql}");
					}
				}
			}

			private void VisitPossiblyAliasedColumn(ColumnExpression queryExpression)
			{
				if (queryExpression.Source is AliasExpression sourceAliasExpression)
				{
					Visit(QueryExpression.Column(queryExpression.ColumnName, QueryExpression.Table(sourceAliasExpression.Identifier.Identifier)));
					return;
				}
				Visit(queryExpression);
			}
		}
	}
}
