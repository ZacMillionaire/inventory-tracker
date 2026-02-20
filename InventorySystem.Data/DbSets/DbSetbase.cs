using Microsoft.Data.Sqlite;

namespace InventorySystem.Data.DbSets;

public class DbSetbase
{
	private readonly SqliteConnection _connection;

	protected DbSetbase(SqliteConnection connection)
	{
		_connection = connection;
	}

	protected T RunInConnection<T>(Func<SqliteConnection, T> func)
	{
		try
		{
			_connection.Open();
			return func(_connection);
		}
		finally
		{
			_connection.Close();
		}
	}
}