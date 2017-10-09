namespace Silk.Data.SQL.Expressions
{
	public enum BitwiseOperator
	{
		And,
		Or,
		ExclusiveOr
	}

	public class BitwiseOperationQueryExpression : BinaryQueryExpression
	{
		public BitwiseOperator Operator { get; }

		public BitwiseOperationQueryExpression(QueryExpression left, BitwiseOperator @operator, QueryExpression right)
			: base(left, right, BinaryOperation.Bitwise)
		{
			Operator = @operator;
		}
	}
}
