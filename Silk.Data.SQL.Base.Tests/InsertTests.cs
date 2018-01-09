using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class InsertTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void SingleInsert()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Insert(
					"TestTable",
					new[] { "Column1", "Column2" },
					new object[] { 1, "Hello World" }
				));
			Assert.AreEqual("INSERT INTO [TestTable] ([Column1], [Column2])  VALUES  ( @valueParameter1 ,  @valueParameter2 ) ; ", sqlQuery.SqlText);
			Assert.AreEqual(1, sqlQuery.QueryParameters["valueParameter1"].Value);
			Assert.AreEqual("Hello World", sqlQuery.QueryParameters["valueParameter2"].Value);
		}

		[TestMethod]
		public void BulkInsert()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Insert(
					"TestTable",
					new[] { "Column1", "Column2" },
					new object[] { 1, "Hello" },
					new object[] { 2, "World" },
					new object[] { 3, "Hello World" }
				));
			Assert.AreEqual("INSERT INTO [TestTable] ([Column1], [Column2])  VALUES  ( @valueParameter1 ,  @valueParameter2 ) ,  ( @valueParameter3 ,  @valueParameter4 ) ,  ( @valueParameter5 ,  @valueParameter6 ) ; ", sqlQuery.SqlText);
			Assert.AreEqual(1, sqlQuery.QueryParameters["valueParameter1"].Value);
			Assert.AreEqual("Hello", sqlQuery.QueryParameters["valueParameter2"].Value);
			Assert.AreEqual(2, sqlQuery.QueryParameters["valueParameter3"].Value);
			Assert.AreEqual("World", sqlQuery.QueryParameters["valueParameter4"].Value);
			Assert.AreEqual(3, sqlQuery.QueryParameters["valueParameter5"].Value);
			Assert.AreEqual("Hello World", sqlQuery.QueryParameters["valueParameter6"].Value);
		}
	}
}
