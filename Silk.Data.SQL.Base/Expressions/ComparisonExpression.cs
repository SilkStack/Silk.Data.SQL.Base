namespace Silk.Data.SQL.Expressions
{
	public enum ComparisonOperator
	{
		AreEqual,
		AreNotEqual,
		GreaterThan,
		LessThan,
		GreaterThanOrEqualTo,
		LessThanOrEqualTo,
		Like,
		None
	}

	public class ComparisonExpression : BinaryQueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Binary;

		public ComparisonOperator Operator { get; }

		public ComparisonExpression(QueryExpression left, ComparisonOperator @operator, QueryExpression right) :
			base(left, right, BinaryOperation.Comparison)
		{
			Operator = @operator;
		}
	}
}
