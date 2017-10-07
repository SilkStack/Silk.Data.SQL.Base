namespace Silk.Data.SQL.Expressions
{
	public class InsertExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; }
		public ColumnExpression[] Columns { get; }
		public QueryExpression[][] RowsExpressions { get; }

		public InsertExpression(TableExpression table, ColumnExpression[] columns,
			QueryExpression[][] rowsExpressions)
		{
			Table = table;
			Columns = columns;
			RowsExpressions = rowsExpressions;
		}
	}
}
