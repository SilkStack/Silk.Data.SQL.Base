using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class TransactionTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void TransactionsWithParameters()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
					QueryExpression.Transaction(
						QueryExpression.Delete(
							QueryExpression.Table("TestTable"),
							QueryExpression.Compare(QueryExpression.Column("Id"), ComparisonOperator.AreEqual, QueryExpression.Value(1))
						),
						QueryExpression.Delete(
							QueryExpression.Table("TestTable"),
							QueryExpression.Compare(QueryExpression.Column("Id"), ComparisonOperator.AreEqual, QueryExpression.Value(2))
						)
				));
			Assert.AreEqual(@"BEGIN TRANSACTION;
DELETE FROM [TestTable] WHERE ([Id] =  @valueParameter1 );
DELETE FROM [TestTable] WHERE ([Id] =  @valueParameter2 );
COMMIT;
", sqlQuery.SqlText);
		}
	}
}
