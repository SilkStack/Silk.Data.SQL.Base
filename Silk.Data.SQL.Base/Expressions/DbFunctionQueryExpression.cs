namespace Silk.Data.SQL.Expressions
{
	public abstract class DbFunctionQueryExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.DbFunction;
	}
}
