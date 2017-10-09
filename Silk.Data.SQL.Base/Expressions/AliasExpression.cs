namespace Silk.Data.SQL.Expressions
{
	public class AliasExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Alias;

		public QueryExpression Expression { get; }
		public AliasIdentifierExpression Identifier { get; }

		public AliasExpression(QueryExpression expression, string aliasName)
		{
			Expression = expression;
			Identifier = new AliasIdentifierExpression(aliasName);
		}
	}
}
