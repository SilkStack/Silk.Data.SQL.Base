using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class FunctionTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void DistinctFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(new[] {
					QueryExpression.Distinct(QueryExpression.All())
				}));
			Assert.AreEqual("SELECT  DISTINCT *", sqlQuery.SqlText);
		}

		[TestMethod]
		public void CountFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(new[] {
					QueryExpression.CountFunction()
				}));
			Assert.AreEqual("SELECT  COUNT() ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void CountWithParameterFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(new[] {
					QueryExpression.CountFunction(QueryExpression.All())
				}));
			Assert.AreEqual("SELECT  COUNT(*) ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void RandomFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(new[] {
					QueryExpression.Random()
				}));
			Assert.AreEqual("SELECT  RAND() ", sqlQuery.SqlText);
		}
	}
}
