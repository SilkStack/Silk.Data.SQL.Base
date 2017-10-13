namespace Silk.Data.SQL.Expressions
{
	public class TableExistsVirtualFunctionExpression : DbFunctionQueryExpression
	{
		public new TableExpression Table { get; }

		public TableExistsVirtualFunctionExpression(TableExpression table)
		{
			Table = table;
		}
	}
}
