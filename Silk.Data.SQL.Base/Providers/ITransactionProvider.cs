using System;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Providers
{
	public interface ITransactionProvider : IDisposable
	{
		Transaction CreateTransaction();
		Task<Transaction> CreateTransactionAsync();
	}
}
