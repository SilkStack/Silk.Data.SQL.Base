using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class CreateTableTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void CreateTable()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.CreateTable(
					"TestTable",
					QueryExpression.DefineColumn("Column1", SqlDataType.Int(), isAutoIncrement: true, isPrimaryKey: true),
					QueryExpression.DefineColumn("Column2", SqlDataType.Text(255), isPrimaryKey: true),
					QueryExpression.DefineColumn("Column3", SqlDataType.Text())
				));
			Assert.AreEqual("CREATE TABLE [TestTable] ([Column1] Int() NOT NULL AUTOINC, [Column2] Text(255) NOT NULL, [Column3] Text(), CONSTRAINT [PK] PRIMARY KEY ([Column1],[Column2]))", sqlQuery.SqlText);
		}
	}
}
