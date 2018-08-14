using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silk.Data.SQL.Base.Tests
{
	[TestClass]
	public class ArithmaticTests
	{
		private TestQueryConverter _queryConverter = new TestQueryConverter();

		[TestMethod]
		public void Addition()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Add(QueryExpression.Value(1), QueryExpression.Value(2))
				));
			Assert.AreEqual("SELECT ( @valueParameter1  +  @valueParameter2 ); ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void Subtraction()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Subtract(QueryExpression.Value(1), QueryExpression.Value(2))
				));
			Assert.AreEqual("SELECT ( @valueParameter1  -  @valueParameter2 ); ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void Multiplication()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Multiply(QueryExpression.Value(1), QueryExpression.Value(2))
				));
			Assert.AreEqual("SELECT ( @valueParameter1  *  @valueParameter2 ); ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void Division()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Divide(QueryExpression.Value(1), QueryExpression.Value(2))
				));
			Assert.AreEqual("SELECT ( @valueParameter1  /  @valueParameter2 ); ", sqlQuery.SqlText);
		}

		[TestMethod]
		public void Nested()
		{
			var sqlQuery = _queryConverter.ConvertToQuery(
				QueryExpression.Select(
					projection: QueryExpression.Add(QueryExpression.Value(1), QueryExpression.Multiply(QueryExpression.Value(2), QueryExpression.Value(3)))
				));
			Assert.AreEqual("SELECT ( @valueParameter1  + ( @valueParameter2  *  @valueParameter3 )); ", sqlQuery.SqlText);
		}
	}
}
