namespace Silk.Data.SQL.Expressions
{
	public class CreateTableIndexExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public new TableExpression Table { get; set; }
		public ColumnExpression[] Columns { get; set; }
		public bool UniqueConstraint { get; set; }

		public CreateTableIndexExpression(TableExpression table, ColumnExpression[] columns, bool uniqueConstraint)
		{
			Table = table;
			Columns = columns;
			UniqueConstraint = uniqueConstraint;
		}
	}
}
