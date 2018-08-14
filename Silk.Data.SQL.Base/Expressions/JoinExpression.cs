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
		
		public QueryExpression Source { get; }
		public JoinDirection Direction { get; }
		public QueryExpression OnCondition { get; }

		public JoinExpression(QueryExpression source,
			JoinDirection direction, QueryExpression onCondition)
		{
			Source = source;
			Direction = direction;
			OnCondition = onCondition;
		}

	}
}
