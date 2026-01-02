using System.ComponentModel;
using InventorySystem.Data.DbSets;
using InventorySystem.Data.Migrations;
using Microsoft.Data.Sqlite;

namespace InventorySystem.Data;

public sealed class DatabaseContext
{
	private readonly SqliteConnection _connection;

	[Description("Used for testing")]
	internal readonly string DatabaseLocation;

	[Description("Used for testing")]
	internal SqliteConnection Connection => _connection;

	public readonly AttributeSet Attributes;

	public DatabaseContext(string connectionString)
	{
		_connection = new SqliteConnection(connectionString);

		_connection.Open();
		DatabaseLocation = _connection.DataSource;
		_connection.Close();

		RunMigrations();

		Attributes = new AttributeSet(_connection);
	}

	private void RunMigrations()
	{
		// TODO: store migrations in a table, only run missing migrations
		AttributesTableMigration.Up(_connection);
	}

	public void Seed(Action<SqliteConnection> seed)
	{
		//seed(_connection);
	}
}