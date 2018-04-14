using System;

namespace Silk.Data.SQL.Providers
{
	public interface ITransaction : IQueryProvider, IDisposable
	{
		void Commit();
		void Rollback();
	}
}
