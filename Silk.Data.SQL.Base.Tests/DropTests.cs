using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class DropTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void DropTable()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.DropTable("TestTable")
				);
			Assert.AreEqual("DROP TABLE [TestTable]; ", sqlQuery.SqlText);
		}
	}
}
