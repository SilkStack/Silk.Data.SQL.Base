namespace Silk.Data.SQL.Expressions
{
	public interface IExtensionExpression
	{
		void Visit(QueryExpressionVisitor visitor);
	}
}
