namespace Silk.Data.SQL.Expressions
{
	public enum BinaryOperation
	{
		Comparison,
		Condition,
		Arithmatic,
		Bitwise
	}

	public abstract class BinaryQueryExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Binary;

		public QueryExpression Left { get; }
		public QueryExpression Right { get; }
		public BinaryOperation BinaryOperationType { get; }

		protected BinaryQueryExpression(QueryExpression left, QueryExpression right, BinaryOperation binaryOperationType)
		{
			Left = left;
			Right = right;
			BinaryOperationType = binaryOperationType;
		}
	}
}
