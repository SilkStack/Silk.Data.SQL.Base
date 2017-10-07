﻿using Silk.Data.SQL.Expressions;
using System;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Providers
{
	/// <summary>
	/// Database provider.
	/// </summary>
	public interface IDataProvider : IDisposable
	{
		/// <summary>
		/// Gets the provider's name.
		/// </summary>
		string ProviderName { get; }

		/// <summary>
		/// Execute a query and return the number of effected rows.
		/// </summary>
		/// <param name="queryExpression"></param>
		int ExecuteNonQuery(QueryExpression queryExpression);
		/// <summary>
		/// Execute a query and return the number of effected rows
		/// </summary>
		/// <param name="queryExpression"></param>
		/// <returns></returns>
		Task<int> ExecuteNonQueryAsync(QueryExpression queryExpression);
	}
}
