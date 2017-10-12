namespace Silk.Data.SQL.Expressions
{
	public class ExecuteStoredProcedureExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public string StoredProcedureName { get; }
		public QueryExpression[] Arguments { get; }

		public ExecuteStoredProcedureExpression(string storedProcedureName, QueryExpression[] arguments)
		{
			StoredProcedureName = storedProcedureName;
			Arguments = arguments;
		}
	}
}
