using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class ComparisonTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void ConvertNullComparisonToIsNull()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Compare(
						QueryExpression.Column("Column"),
						ComparisonOperator.AreEqual,
						QueryExpression.Value(null)
						)
				));
			Assert.AreEqual("SELECT ([Column] IS NULL ); ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void ConvertNullComparisonToIsNotNull()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Compare(
						QueryExpression.Column("Column"),
						ComparisonOperator.AreNotEqual,
						QueryExpression.Value(null)
						)
				));
			Assert.AreEqual("SELECT ([Column] IS NOT NULL ); ", sqlQuery.SqlText);
		}
	}
}
