using InventorySystem.Data.Entities;
using Microsoft.Data.Sqlite;

namespace InventorySystem.Data;

public class DatabaseContext
{
	private readonly SqliteConnection _connection;

	public DatabaseContext(string connectionString)
	{
		_connection = new SqliteConnection(connectionString);
		
		_connection.Open();

		var a = _connection.CreateCommand();
		a.CommandText = """
		                create table Attributes
		                (
		                    Id      integer
		                        constraint Attributes_pk
		                            primary key autoincrement,
		                    Name    TEXT    not null,
		                    KeyName TEXT,
		                    Type    integer not null
		                );

		                create unique index Idx_KeyName
		                    on Attributes (KeyName);

		                create index Idx_Name
		                    on Attributes (Name);

		                create index Idx_Type
		                    on Attributes (Type);
		                """;
		a.ExecuteNonQuery();
		_connection.Close();
	}

	public void Seed(Action<SqliteConnection> seed)
	{
		seed(_connection);
	}

	internal EntityAttribute CreateEntity(EntityAttribute entityAttribute)
	{
		//_connection.Insert(entityAttribute);
		_connection.Open();

		var insertCommand = _connection.CreateCommand();
		insertCommand.CommandText = """
		                            INSERT INTO Attributes (Name, KeyName, Type) VALUES ($name, $keyName, $type) Returning RowId
		                            """;
		insertCommand.Parameters.AddRange([
			new SqliteParameter("$name", entityAttribute.Name),
			new SqliteParameter("keyName", entityAttribute.KeyName),
			new SqliteParameter("type", entityAttribute.Type),
		]);
		
		var insertedId = insertCommand.ExecuteScalar();
		
		_connection.Close();

		return entityAttribute;
	}
}