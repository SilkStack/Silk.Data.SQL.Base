using Silk.Data.SQL.Queries;

namespace Silk.Data.SQL.Base.Tests
{
	public class TestQueryConverter : QueryConverterCommonBase
	{
		protected override string ProviderName => "tests";

		protected override string QuoteIdentifier(string schemaComponent)
		{
			return $"[{schemaComponent}]";
		}
	}
}
