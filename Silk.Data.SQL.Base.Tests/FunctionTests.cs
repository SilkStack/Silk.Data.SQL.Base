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
				QueryExpression.Select(
					projection: QueryExpression.Distinct(QueryExpression.All())
				));
			Assert.AreEqual("SELECT  DISTINCT *; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void CountFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.CountFunction()
				));
			Assert.AreEqual("SELECT  COUNT() ; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void CountWithParameterFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.CountFunction(QueryExpression.All())
				));
			Assert.AreEqual("SELECT  COUNT(*) ; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void RandomFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Random()
				));
			Assert.AreEqual("SELECT  RANDOM(); ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void ConcatFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Concat(QueryExpression.Value("Hello"), QueryExpression.Value(" "), QueryExpression.Value("World"))
				));
			Assert.AreEqual("SELECT  CONCAT(  @valueParameter1 ,  @valueParameter2 ,  @valueParameter3 ) ; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void MinFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Min()
				));
			Assert.AreEqual("SELECT  MIN() ; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void MinWithParameterFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Min(QueryExpression.All())
				));
			Assert.AreEqual("SELECT  MIN(*) ; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void MaxFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Max()
				));
			Assert.AreEqual("SELECT  MAX() ; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void MaxWithParameterFunction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Max(QueryExpression.All())
				));
			Assert.AreEqual("SELECT  MAX(*) ; ", sqlQuery.SqlText);
		}
	}
}
