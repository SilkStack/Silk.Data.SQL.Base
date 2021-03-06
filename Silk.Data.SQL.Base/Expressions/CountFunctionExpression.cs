﻿namespace Silk.Data.SQL.Expressions
{
	public class CountFunctionExpression : DbFunctionQueryExpression
	{
		public QueryExpression Expression { get; }

		public CountFunctionExpression(QueryExpression expression = null)
		{
			Expression = expression;
		}
	}
}
