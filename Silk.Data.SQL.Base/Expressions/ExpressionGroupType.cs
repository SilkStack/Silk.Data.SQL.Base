namespace Silk.Data.SQL.Expressions
{
	public enum ExpressionGroupType
	{
		Projection,
		Joins,
		GroupByClauses,
		OrderByClauses,
		ColumnList,
		RowValues,
		RowAssignments,
		ProcedureArguments,
		Queries,
		ColumnDefinitions
	}
}
