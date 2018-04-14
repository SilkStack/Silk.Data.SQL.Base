using System;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Providers
{
	/// <summary>
	/// Database provider.
	/// </summary>
	public interface IDataProvider : IQueryProvider, IDisposable
	{
		ITransaction CreateTransaction();
		Task<ITransaction> CreateTransactionAsync();
	}
}
