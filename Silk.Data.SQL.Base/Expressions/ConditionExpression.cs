namespace Silk.Data.SQL.Expressions
{
	public class ConditionExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Condition;

		public QueryExpression Left { get; }
		public ConditionOperator Operator { get; }
		public QueryExpression Right { get; }

		public ConditionExpression(QueryExpression left, ConditionOperator @operator, QueryExpression right)
		{
			Left = left;
			Operator = @operator;
			Right = right;
		}
	}
}
