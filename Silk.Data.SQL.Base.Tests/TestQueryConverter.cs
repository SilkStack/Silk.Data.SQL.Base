using Silk.Data.SQL.Queries;
using System;

namespace Silk.Data.SQL.Base.Tests
{
	public class TestQueryConverter : QueryConverterCommonBase
	{
		protected override string ProviderName => "tests";

		protected override string AutoIncrementSql => "AUTOINC";

		protected override string GetDbDatatype(SqlDataType sqlDataType)
		{
			return $"{sqlDataType.BaseType}({string.Join(",", sqlDataType.Parameters)})";
		}

		protected override string QuoteIdentifier(string schemaComponent)
		{
			if (schemaComponent == "*")
				return "*";
			return $"[{schemaComponent}]";
		}
	}
}
