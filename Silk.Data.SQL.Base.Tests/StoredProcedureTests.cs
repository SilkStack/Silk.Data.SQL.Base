using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class StoredProcedureTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void SprocWithoutArguments()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
					QueryExpression.ExecuteStoredProcedure("sp_Test")
				);
			Assert.AreEqual("EXECUTE [sp_Test] ; ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void SprocWithArguments()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
					QueryExpression.ExecuteStoredProcedure(
						"sp_Test",
						QueryExpression.Value(1),
						QueryExpression.Value(5)
				));
			Assert.AreEqual("EXECUTE [sp_Test]  @valueParameter1 ,  @valueParameter2 ; ", sqlQuery.SqlText);
		}
	}
}
