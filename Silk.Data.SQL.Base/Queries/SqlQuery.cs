using System.Collections.Generic;

namespace Silk.Data.SQL.Queries
{
	/// <summary>
	/// An sql query prepared to execute on a provider.
	/// </summary>
	public class SqlQuery
	{
		/// <summary>
		/// Gets the provider's name.
		/// </summary>
		public string ProviderName { get; }

		/// <summary>
		/// Gets the sql query text.
		/// </summary>
		public string SqlText { get; }

		/// <summary>
		/// Gets a dictionary of query parameters indexed by parameter name.
		/// </summary>
		public Dictionary<string, QueryParameter> QueryParameters { get; }

		public SqlQuery(string providerName, string sqlText,
			Dictionary<string, QueryParameter> queryParameters)
		{
			ProviderName = providerName;
			SqlText = sqlText;
			QueryParameters = queryParameters;
		}
	}
}
