using System.ComponentModel;
using System.Runtime.CompilerServices;
using InventorySystem.Data.Entities;
using InventorySystem.Data.Migrations;
using Microsoft.Data.Sqlite;

namespace InventorySystem.Data;

public class DatabaseContext
{
	private readonly SqliteConnection _connection;

	[Description("Used for testing")]
	internal readonly string DatabaseLocation;

	public DatabaseContext(string connectionString)
	{
		_connection = new SqliteConnection(connectionString);
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

	internal EntityAttribute CreateEntity(EntityAttribute entityAttribute)
	{
		try
		{
			_connection.Open();

			var insertCommand = _connection.CreateCommand();
			insertCommand.CommandText = """
			                            INSERT INTO Attributes (Id, Name, KeyName, Type) VALUES ($Id, $name, $keyName, $type) Returning RowId
			                            """;
			insertCommand.Parameters.AddRange([
				new SqliteParameter("id", entityAttribute.Id),
				new SqliteParameter("name", entityAttribute.Name),
				new SqliteParameter("keyName", entityAttribute.KeyName),
				new SqliteParameter("type", entityAttribute.Type),
			]);

			var insertedId = insertCommand.ExecuteScalar();

			return entityAttribute;
		}
		finally
		{
			_connection.Close();
		}
	}
}