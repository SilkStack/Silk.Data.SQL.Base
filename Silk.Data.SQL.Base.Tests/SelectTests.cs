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
				where: QueryExpression.Compare(
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
				where: QueryExpression.AndAlso(
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Value("columnValue")
					)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] WHERE ([TestTable].[TestColumn1] AND  @valueParameter1 )", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectWithHaving()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				having: QueryExpression.Compare(
					QueryExpression.Column("TestColumn1", tableExpression),
					ComparisonOperator.AreEqual,
					QueryExpression.Value("columnValue")
					)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] HAVING ([TestTable].[TestColumn1] =  @valueParameter1 )", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectWithGroupBy()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				groupBy: new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				}
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] GROUP BY [TestTable].[TestColumn1], [TestTable].[TestColumn2]", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectWithOrderBy()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				orderBy: new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				}
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] ORDER BY [TestTable].[TestColumn1], [TestTable].[TestColumn2]", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectWithLimit()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				limit: QueryExpression.Value(1)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] LIMIT  @valueParameter1 ", sqlQuery.SqlText);
			Assert.AreEqual(1, sqlQuery.QueryParameters["valueParameter1"].Value);
		}

		[TestMethod]
		public void SelectWithLimitAndOffset()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				limit: QueryExpression.Value(1),
				offset: QueryExpression.Value(2)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] LIMIT  @valueParameter1  OFFSET  @valueParameter2 ", sqlQuery.SqlText);
			Assert.AreEqual(1, sqlQuery.QueryParameters["valueParameter1"].Value);
			Assert.AreEqual(2, sqlQuery.QueryParameters["valueParameter2"].Value);
		}

		[TestMethod]
		public void SelectWithOffsetWithoutLimit()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", tableExpression)
				},
				from: tableExpression,
				offset: QueryExpression.Value(2)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable].[TestColumn2] FROM [TestTable] LIMIT  @valueParameter1  OFFSET  @valueParameter2 ", sqlQuery.SqlText);
			Assert.AreEqual(int.MaxValue, sqlQuery.QueryParameters["valueParameter1"].Value);
			Assert.AreEqual(2, sqlQuery.QueryParameters["valueParameter2"].Value);
		}

		[TestMethod]
		public void SelectWithJoin()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var joinTableExpression = QueryExpression.Table("TestTable2");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.Column("TestColumn1", tableExpression),
					QueryExpression.Column("TestColumn2", joinTableExpression)
				},
				from: tableExpression,
				joins: new []
				{
					QueryExpression.Join(
						QueryExpression.Column("TestColumn1", tableExpression),
						QueryExpression.Column("TestColumn2", joinTableExpression)
						)
				}
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [TestTable].[TestColumn1], [TestTable2].[TestColumn2] FROM [TestTable] INNER JOIN [TestTable2] ON [TestTable].[TestColumn1] = [TestTable2].[TestColumn2]", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SelectWithMultipleJoins()
		{
			var tableExpression = QueryExpression.Table("TestTable");
			var joinTableExpression = QueryExpression.Table("TestTable2");
			var joinAliasExpression1 = QueryExpression.Alias(joinTableExpression, "t2");
			var joinAliasExpression2 = QueryExpression.Alias(joinTableExpression, "t3");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.All()
				},
				from: tableExpression,
				joins: new[]
				{
					QueryExpression.Join(
						QueryExpression.Column("TestColumn1", tableExpression),
						QueryExpression.Column("TestColumn2", joinAliasExpression1)
						),
					QueryExpression.Join(
						QueryExpression.Column("TestColumn1", tableExpression),
						QueryExpression.Column("TestColumn3", joinAliasExpression2)
						)
				}
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT * FROM [TestTable] INNER JOIN [TestTable2] AS [t2] ON [TestTable].[TestColumn1] = [t2].[TestColumn2] INNER JOIN [TestTable2] AS [t3] ON [TestTable].[TestColumn1] = [t3].[TestColumn3]", sqlQuery.SqlText);
		}

		[TestMethod]
		public void ComplicatedSelect()
		{
			var sourceAliasExpression = QueryExpression.Alias(QueryExpression.Select(
				new[] { QueryExpression.All() },
				from: QueryExpression.Table("TestTable")
				), "source");
			var joinTableExpression = QueryExpression.Table("TestTable2");
			var joinAliasExpression1 = QueryExpression.Alias(joinTableExpression, "t2");
			var joinAliasExpression2 = QueryExpression.Alias(joinTableExpression, "t3");
			var queryExpression = QueryExpression.Select(
				new[] {
					QueryExpression.All(sourceAliasExpression.Identifier),
					QueryExpression.All(joinAliasExpression1.Identifier)
				},
				from: sourceAliasExpression,
				joins: new[]
				{
					QueryExpression.Join(
						QueryExpression.Column("TestColumn1", sourceAliasExpression),
						QueryExpression.Column("TestColumn2", joinAliasExpression1)
						),
					QueryExpression.Join(
						QueryExpression.Column("TestColumn1", sourceAliasExpression),
						QueryExpression.Column("TestColumn3", joinAliasExpression2)
						)
				},
				where: QueryExpression.Compare(QueryExpression.Column("TestColumn3", joinAliasExpression2.Identifier), ComparisonOperator.AreEqual, QueryExpression.Value(1)),
				orderBy: new[] { QueryExpression.Column("TestColumn2", joinAliasExpression1.Identifier) },
				groupBy: new[] { QueryExpression.Column("TestColumn1", sourceAliasExpression.Identifier) },
				offset: QueryExpression.Value(5),
				limit: QueryExpression.Value(100)
				);

			var sqlQuery = _queryConverter.ConvertToQuery(queryExpression);
			Assert.AreEqual("SELECT [source].*, [t2].* FROM (SELECT * FROM [TestTable])  AS [source] INNER JOIN [TestTable2] AS [t2] ON [source].[TestColumn1] = [t2].[TestColumn2] INNER JOIN [TestTable2] AS [t3] ON [source].[TestColumn1] = [t3].[TestColumn3] WHERE ([t3].[TestColumn3] =  @valueParameter1 ) GROUP BY [source].[TestColumn1] ORDER BY [t2].[TestColumn2] LIMIT  @valueParameter2  OFFSET  @valueParameter3 ", sqlQuery.SqlText);
		}
	}
}
