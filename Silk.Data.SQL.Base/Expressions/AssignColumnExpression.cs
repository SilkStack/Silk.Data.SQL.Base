namespace Silk.Data.SQL.Expressions
{
	public class AssignColumnExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Assignment;

		public new ColumnExpression Column { get; }
		public QueryExpression Expression { get; }

		public AssignColumnExpression(ColumnExpression column, QueryExpression expression)
		{
			Column = column;
			Expression = expression;
		}
	}
}
