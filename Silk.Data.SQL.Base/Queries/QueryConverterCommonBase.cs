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
							VisitExpressionGroup(select.Joins, ExpressionGroupType.Joins);
						if (select.WhereConditions != null)
						{
							Sql.Append(" WHERE ");
							Visit(select.WhereConditions);
						}
						if (select.GroupConditions != null && select.GroupConditions.Length > 0)
							VisitExpressionGroup(select.GroupConditions, ExpressionGroupType.GroupByClauses);
						if (select.HavingConditions != null)
							Visit(select.HavingConditions);
						if (select.OrderConditions != null && select.OrderConditions.Length > 0)
							VisitExpressionGroup(select.OrderConditions, ExpressionGroupType.OrderByClauses);
						if (select.Limit != null)
							Visit(select.Limit);
						if (select.Offset != null)
							Visit(select.Offset);
						break;
					case InsertExpression insert:
						Visit(insert.Table);
						VisitExpressionGroup(insert.Columns, ExpressionGroupType.ColumnList);
						foreach (var rowExpressions in insert.RowsExpressions)
							VisitExpressionGroup(rowExpressions, ExpressionGroupType.RowValues);
						break;
					case UpdateExpression update:
						Visit(update.Table);
						VisitExpressionGroup(update.Assignments, ExpressionGroupType.RowAssignments);
						Visit(update.WhereConditions);
						break;
					case DeleteExpression delete:
						Visit(delete.Table);
						Visit(delete.WhereConditions);
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
					case ExpressionGroupType.Projection:
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
					case ExpressionGroupType.Joins:
						{
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

				Visit(binaryExpression.Right);
				Sql.Append(")");
			}
		}
	}
}
