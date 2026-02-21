using System.ComponentModel;
using InventorySystem.Data.DbSets;
using InventorySystem.Data.Migrations;
using Microsoft.Data.Sqlite;

namespace InventorySystem.Data;

public sealed class DatabaseContext
{
	private readonly SqliteConnection _connection;
	private readonly TimeProvider _timeProvider;

	public readonly ItemSet Items;

	public DatabaseContext(string connectionString, TimeProvider? timeProvider = null)
	{
		_connection = new SqliteConnection(connectionString);
		_timeProvider = timeProvider ?? TimeProvider.System;

		_connection.Open();
		DatabaseLocation = _connection.DataSource;
		_connection.Close();

		RunMigrations();
		
		Items = new ItemSet(_connection, _timeProvider);
	}

	private void RunMigrations()
	{
		// TODO: store migrations in a table, only run missing migrations
		AttributesTableMigration.Up(_connection);
		ItemsTableMigration.Up(_connection);
		StringAttributeValueTableMigration.Up(_connection);
	}

	public void Seed(Action<SqliteConnection> seed)
	{
		//seed(_connection);
	}


	#region Test Internals

	[Description("Used for testing")]
	internal readonly string DatabaseLocation;

	[Description("Used for testing")]
	internal SqliteConnection Connection => _connection;

	#endregion
}