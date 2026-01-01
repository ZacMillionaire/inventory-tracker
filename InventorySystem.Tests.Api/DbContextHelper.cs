using InventorySystem.Data;

namespace InventorySystem.Tests.Api;

public class DbContextHelper : IDisposable
{
	private readonly DatabaseContext _dbcontext;

	public DbContextHelper(string databaseName)
	{
		_dbcontext = new DatabaseContext(databaseName);
	}

	public DatabaseContext GetContext => _dbcontext;

	public void Dispose()
	{
		if (File.Exists(_dbcontext.DatabaseLocation))
		{
			File.Delete(_dbcontext.DatabaseLocation);
		}
	}
}