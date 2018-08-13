namespace Silk.Data.SQL.Expressions
{
	public enum JoinDirection
	{
		Left,
		Right,
		Inner
	}

	public class JoinExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Join;

		public ColumnExpression LeftColumn { get; }
		public ColumnExpression RightColumn { get; }
		public JoinDirection Direction { get; }
		public QueryExpression OnCondition { get; }

		public JoinExpression(ColumnExpression leftColumn, ColumnExpression rightColumn,
			JoinDirection direction, QueryExpression onCondition)
		{
			LeftColumn = leftColumn;
			RightColumn = rightColumn;
			Direction = direction;
			OnCondition = onCondition;
		}

	}
}
