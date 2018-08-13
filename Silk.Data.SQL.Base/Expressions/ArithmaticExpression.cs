namespace Silk.Data.SQL.Expressions
{
	public enum ArithmaticOperator
	{
		Addition,
		Subtraction,
		Multiplication,
		Division
	}

	public class ArithmaticQueryExpression : BinaryQueryExpression
	{
		public ArithmaticOperator Operator { get; }

		public ArithmaticQueryExpression(QueryExpression left, ArithmaticOperator @operator, QueryExpression right)
			: base(left, right, BinaryOperation.Arithmatic)
		{
			Operator = @operator;
		}
	}
}
