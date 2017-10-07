namespace Silk.Data.SQL.Expressions
{
	public class ValueExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Value;

		public new object Value { get; }

		public ValueExpression(object value)
		{
			Value = value;
		}
	}
}
