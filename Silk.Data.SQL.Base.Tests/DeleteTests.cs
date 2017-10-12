using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class DeleteTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void DeleteWithoutWhere()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Delete(
					QueryExpression.Table("TestTable")
				));
			Assert.AreEqual("DELETE FROM [TestTable]", sqlQuery.SqlText);
		}

		[TestMethod]
		public void DeleteWithWhere()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Delete(
					QueryExpression.Table("TestTable"),
					QueryExpression.Compare(QueryExpression.Column("Column1"), ComparisonOperator.AreEqual, QueryExpression.Column("Column2"))
				));
			Assert.AreEqual("DELETE FROM [TestTable] WHERE ([Column1] = [Column2])", sqlQuery.SqlText);
		}
	}
}
