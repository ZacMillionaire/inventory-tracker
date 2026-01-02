using Microsoft.Data.Sqlite;

namespace InventorySystem.Data.Migrations;

public class ItemsTableMigration
{
	public static void Up(SqliteConnection connection)
	{
		try
		{
			connection.Open();
			using var command = connection.CreateCommand();
			command.CommandText = """
			                      create table IF NOT EXISTS Items
			                      (
			                          Id          TEXT    not null
			                              constraint Items_pk
			                                  primary key,
			                          Name        TEXT    not null,
			                          Description TEXT,
			                          CreatedUtc  integer not null,
			                          UpdatedUtc  integer
			                      );

			                      create index IF NOT EXISTS Idx_Items_CreatedUtc
			                          on Items (CreatedUtc);

			                      create index IF NOT EXISTS Idx_Items_Name
			                          on Items (Name);

			                      create index IF NOT EXISTS Idx_Items_UpdatedUtc
			                          on Items (UpdatedUtc);
			                      """;
			command.ExecuteNonQuery();
		}
		finally
		{
			connection.Close();
		}
	}
}