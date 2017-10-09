namespace Silk.Data.SQL.Expressions
{
	public class AliasIdentifierExpression : QueryExpression
	{
		public string Identifier { get; }

		public override ExpressionNodeType NodeType => ExpressionNodeType.SchemaComponent;

		public AliasIdentifierExpression(string identifier)
		{
			Identifier = identifier;
		}
	}
}
