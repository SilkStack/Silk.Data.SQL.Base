using System.Collections.Generic;

namespace Silk.Data.SQL.Expressions
{
	/// <summary>
	/// Visits nodes in a QueryExpression.
	/// </summary>
	public class QueryExpressionVisitor
	{
		public virtual void Visit(QueryExpression queryExpression)
		{
			switch (queryExpression.NodeType)
			{
				case ExpressionNodeType.Query:
					VisitQuery(queryExpression);
					break;
				case ExpressionNodeType.SchemaComponent:
					VisitSchemaComponent(queryExpression);
					break;
				case ExpressionNodeType.Binary:
					VisitBinary(queryExpression);
					break;
				case ExpressionNodeType.Value:
					VisitValue(queryExpression);
					break;
				case ExpressionNodeType.DbFunction:
					VisitFunction(queryExpression);
					break;
				case ExpressionNodeType.Alias:
					VisitAlias(queryExpression);
					break;
				case ExpressionNodeType.Join:
					VisitJoin(queryExpression);
					break;
				case ExpressionNodeType.Assignment:
					VisitAssignment(queryExpression);
					break;
				case ExpressionNodeType.ColumnDefinition:
					VisitColumnDefinition(queryExpression);
					break;
			}
		}

		protected virtual void VisitExpressionGroup(ICollection<QueryExpression> queryExpressions, ExpressionGroupType groupType)
		{
			foreach (var queryExpression in queryExpressions)
			{
				Visit(queryExpression);
			}
		}

		protected virtual void VisitQuery(QueryExpression queryExpression)
		{
			switch (queryExpression)
			{
				case SelectExpression select:
					VisitExpressionGroup(select.Projection, ExpressionGroupType.Projection);
					if (select.From != null)
						Visit(select.From);
					if (select.Joins != null && select.Joins.Length > 0)
						VisitExpressionGroup(select.Joins, ExpressionGroupType.Joins);
					if (select.WhereConditions != null)
						Visit(select.WhereConditions);
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
				case TransactionExpression transaction:
					VisitExpressionGroup(transaction.Queries, ExpressionGroupType.Queries);
					break;
				case CreateTableExpression create:
					VisitExpressionGroup(create.ColumnDefinitions, ExpressionGroupType.ColumnDefinitions);
					break;
			}
		}

		protected virtual void VisitSchemaComponent(QueryExpression queryExpression)
		{
		}

		protected virtual void VisitBinary(QueryExpression queryExpression)
		{
			if (queryExpression is ComparisonExpression conditionExpression)
			{
				Visit(conditionExpression.Left);
				Visit(conditionExpression.Right);
			}
		}

		protected virtual void VisitValue(QueryExpression queryExpression)
		{
		}

		protected virtual void VisitFunction(QueryExpression queryExpression)
		{
		}

		protected virtual void VisitAlias(QueryExpression queryExpression)
		{
			if (queryExpression is AliasExpression aliasExpression)
			{
				Visit(aliasExpression.Expression);
			}
		}

		protected virtual void VisitJoin(QueryExpression queryExpression)
		{
			if (queryExpression is JoinExpression joinExpression)
			{
				Visit(joinExpression.LeftColumn);
				Visit(joinExpression.RightColumn);
			}
		}

		protected virtual void VisitAssignment(QueryExpression queryExpression)
		{
		}

		protected virtual void VisitColumnDefinition(QueryExpression queryExpression)
		{
		}
	}
}
