using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;
using System.Linq;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class SelectTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void SelectConstantValue()
		{
			var queryExpression = QueryExpression.Select(new[] { QueryExpression.Value(1) });
			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);

			Assert.IsNotNull(sqlQuery);
			Assert.IsNotNull(sqlQuery.QueryParameters);
			Assert.AreEqual(1, sqlQuery.QueryParameters.Count);

			var valueParameter = sqlQuery.QueryParameters.Values.First();
			Assert.AreEqual(1, valueParameter.Value);
			Assert.AreEqual($"SELECT  @{valueParameter.Name} ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectFromTable()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression
				);
			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);

			Assert.IsNotNull(sqlQuery);
			Assert.IsNull(sqlQuery.QueryParameters);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable]", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectFromSubQuery()
		{
			var subSelect = QueryExpression.Alias(
				QueryExpression.Select(new[] { QueryExpression.Value(1) }),
				"subSelect"
				);
			var queryExpression = QueryExpression.Select(
				new[] { QueryExpression.All(subSelect) },
				from: subSelect
				);
			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.Fail("Test not implemented.");
		}
	}
}
