using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class UpdateTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void UpdateWithoutWhere()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Update(
					QueryExpression.Table("TestTable"),
					null,
					QueryExpression.Assign("Column1", 1),
					QueryExpression.Assign("Column2", QueryExpression.Column("Column3"))
				));
			Assert.AreEqual("UPDATE [TestTable] SET [Column1] =  @valueParameter1 , [Column2] = [Column3]; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void UpdateWithWhere()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Update(
					QueryExpression.Table("TestTable"),
					QueryExpression.Compare(QueryExpression.Column("Column4"), ComparisonOperator.GreaterThan, QueryExpression.Value(5)),
					QueryExpression.Assign("Column1", 1),
					QueryExpression.Assign("Column2", QueryExpression.Column("Column3"))
				));
			Assert.AreEqual("UPDATE [TestTable] SET [Column1] =  @valueParameter1 , [Column2] = [Column3] WHERE ([Column4] >  @valueParameter2 ); ", sqlQuery.SqlText);
		}
	}
}
