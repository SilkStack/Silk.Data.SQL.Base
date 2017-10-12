using Silk.Data.SQL.Expressions;
using System.Text;
using System.Collections.Generic;

namespace Silk.Data.SQL.Queries
{
	public abstract class QueryConverterCommonBase : IQueryConverter
	{
		protected abstract string ProviderName { get; }

		protected StringBuilder Sql { get; }
		protected QueryWriter ExpressionWriter { get; }

		public QueryConverterCommonBase()
		{
			Sql = new StringBuilder();
			ExpressionWriter = new QueryWriter(Sql, this);
		}

		public SqlQuery ConvertToQuery(QueryExpression queryExpression)
		{
			Sql.Clear();
			ExpressionWriter.Parameters = null;
			ExpressionWriter.Visit(queryExpression);

			return new SqlQuery(ProviderName, Sql.ToString(), ExpressionWriter.Parameters);
		}

		protected virtual string GetConditionOperatorString(ComparisonOperator @operator)
		{
			switch (@operator)
			{
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
					Sql.Append(" RAND(");
					if (randomExpression.Expression != null)
						ExpressionWriter.Visit(randomExpression.Expression);
					Sql.Append(") ");
					break;
			}
		}

		protected abstract string QuoteIdentifier(string schemaComponent);

		protected class QueryWriter : QueryExpressionVisitor
		{
			protected StringBuilder Sql { get; }
			public QueryConverterCommonBase Converter { get; }
			public Dictionary<string,QueryParameter> Parameters { get; set; }

			public QueryWriter(StringBuilder sql, QueryConverterCommonBase converter)
			{
				Sql = sql;
				Converter = converter;
			}

			protected override void VisitQuery(QueryExpression queryExpression)
			{
				var isSubQuery = Sql.Length > 0;
				if (isSubQuery)
				{
					Sql.Append("(");
				}

				switch (queryExpression)
				{
					case SelectExpression select:
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
						break;
					case InsertExpression insert:
						Sql.Append("INSERT INTO ");
						Visit(insert.Table);
						VisitExpressionGroup(insert.Columns, ExpressionGroupType.ColumnList);
						Sql.Append(" VALUES ");
						for (var i = 0; i < insert.RowsExpressions.Length; i++)
						{
							VisitExpressionGroup(insert.RowsExpressions[i], ExpressionGroupType.RowValues);
							if (i < insert.RowsExpressions.Length - 1)
								Sql.Append(", ");
						}
						break;
					case UpdateExpression update:
						Sql.Append("UPDATE ");
						Visit(update.Table);
						Sql.Append(" SET ");
						VisitExpressionGroup(update.Assignments, ExpressionGroupType.RowAssignments);
						if (update.WhereConditions != null)
						{
							Sql.Append(" WHERE ");
							Visit(update.WhereConditions);
						}
						break;
					case DeleteExpression delete:
						Sql.Append("DELETE FROM ");
						Visit(delete.Table);
						if (delete.WhereConditions != null)
						{
							Sql.Append(" WHERE ");
							Visit(delete.WhereConditions);
						}
						break;
					case ExecuteStoredProcedureExpression sprocExec:
						VisitExpressionGroup(sprocExec.Arguments, ExpressionGroupType.ProcedureArguments);
						break;
				}

				if (isSubQuery)
				{
					Sql.Append(") ");
				}
			}

			protected override void VisitExpressionGroup(ICollection<QueryExpression> queryExpressions, ExpressionGroupType groupType)
			{
				switch (groupType)
				{
					case ExpressionGroupType.GroupByClauses:
					case ExpressionGroupType.OrderByClauses:
					case ExpressionGroupType.Projection:
					case ExpressionGroupType.RowAssignments:
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
					case ExpressionGroupType.RowValues:
					case ExpressionGroupType.ColumnList:
						{
							Sql.Append(" (");
							var expressionCount = queryExpressions.Count;
							var i = 0;
							foreach (var expression in queryExpressions)
							{
								i++;
								Visit(expression);
								if (i < expressionCount)
									Sql.Append(", ");
							}
							Sql.Append(") ");
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
					VisitPossiblyAliasedColumn(joinExpression.LeftColumn);
					Sql.Append(" = ");
					VisitPossiblyAliasedColumn(joinExpression.RightColumn);
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
