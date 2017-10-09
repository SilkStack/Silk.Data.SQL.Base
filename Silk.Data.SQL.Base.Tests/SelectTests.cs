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
				new[] { QueryExpression.All(subSelect.Identifier) },
				from: subSelect
				);
			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [subSelect].* FROM (SELECT  @valueParameter1 )  AS [subSelect]", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectWithWhere()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				whereConditions: QueryExpression.Compare(
					QueryExpression.Column("TestColumn1", tableExpression),
					ComparisonOperator.AreEqual,
					QueryExpression.Value("columnValue")
					)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] WHERE ([TestTable].[TestColumn1] =  @valueParameter1 )", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectWithWhereComparison()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				whereConditions: QueryExpression.AndAlso(
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Value("columnValue")
					)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] WHERE ([TestTable].[TestColumn1] AND  @valueParameter1 )", sqlQuery.SqlText);
		}
	}
}
