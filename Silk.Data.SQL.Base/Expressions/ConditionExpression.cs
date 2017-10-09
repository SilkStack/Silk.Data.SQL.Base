namespace Silk.Data.SQL.Expressions
{
	public enum ConditionType
	{
		AndAlso,
		OrElse
	}

	public class ConditionExpression : BinaryQueryExpression
	{
		public ConditionType ConditionType { get; }

		public ConditionExpression(QueryExpression left, ConditionType conditionType, QueryExpression right)
			: base(left, right, BinaryOperation.Condition)
		{
			ConditionType = conditionType;
		}
	}
}
