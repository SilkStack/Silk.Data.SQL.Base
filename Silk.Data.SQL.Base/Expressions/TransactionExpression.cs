using System;
using System.Collections.Generic;
using System.Linq;

namespace Silk.Data.SQL.Expressions
{
	public class TransactionExpression : QueryExpression
	{
		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public QueryExpression[] Queries { get; }

		public TransactionExpression(IEnumerable<QueryExpression> expressions)
		{
			Queries = expressions.ToArray();
			if (Queries.OfType<TransactionExpression>().FirstOrDefault() != null)
				throw new ArgumentException("Nested transaction expressions are not permitted.");
		}
	}
}
