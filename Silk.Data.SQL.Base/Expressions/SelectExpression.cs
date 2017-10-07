namespace Silk.Data.SQL.Expressions
{
	public class SelectExpression : QueryExpression
	{
		public QueryExpression[] Projection { get; }
		public QueryExpression From { get; }
		public QueryExpression[] Joins { get; }
		public ConditionExpression WhereConditions { get; }
		public ConditionExpression HavingConditions { get; }
		public QueryExpression[] OrderConditions { get; }
		public QueryExpression[] GroupConditions { get; }
		public QueryExpression Offset { get; }
		public QueryExpression Limit { get; }

		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public SelectExpression(QueryExpression[] projection,
			QueryExpression from = null,
			QueryExpression[] joins = null,
			ConditionExpression whereConditions = null,
			ConditionExpression havingConditions = null,
			QueryExpression[] orderConditions = null,
			QueryExpression[] groupConditions = null,
			QueryExpression offset = null,
			QueryExpression limit = null)
		{
			Projection = projection;
			From = from;
			Joins = joins;
			WhereConditions = whereConditions;
			HavingConditions = havingConditions;
			OrderConditions = orderConditions;
			Offset = offset;
			Limit = limit;
		}
	}
}
