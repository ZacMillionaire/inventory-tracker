using InventorySystem.Data;
using Microsoft.Data.Sqlite;

namespace InventorySystem.Tests.Api;

public class DbContextHelper : IDisposable
{
	private readonly DatabaseContext _dbcontext;

	public DbContextHelper(string connectionString, TimeProvider timeProvider)
	{
		_dbcontext = new DatabaseContext(connectionString, timeProvider);
	}

	public DatabaseContext GetContext => _dbcontext;

	public void Dispose()
	{
		if (File.Exists(_dbcontext.DatabaseLocation))
		{
			// ensure that all pools are closed so any file locks are removed
			// Not sure if SqliteConnection.ClearAllPools() would be fine with parallel tests but I didn't want to
			// try and find out at the time of writing
			SqliteConnection.ClearPool(_dbcontext.Connection);
			File.Delete(_dbcontext.DatabaseLocation);
		}
	}
}