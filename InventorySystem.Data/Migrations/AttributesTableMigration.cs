using Microsoft.Data.Sqlite;

namespace InventorySystem.Data.Migrations;

public class AttributesTableMigration
{
	public static void Up(SqliteConnection connection)
	{
		try
		{
			connection.Open();
			using var command = connection.CreateCommand();
			command.CommandText = """
			                      create table IF NOT EXISTS 
			                      Attributes
			                      (
			                          Id      TEXT
			                              	  constraint Attributes_pk
			                                  primary key,
			                          Name    TEXT	not null,
			                          KeyName TEXT  not null,
			                          Type    integer not null
			                      );

			                      create unique index IF NOT EXISTS Idx_KeyName
			                          on Attributes (KeyName);

			                      create index IF NOT EXISTS Idx_Name
			                          on Attributes (Name);

			                      create index IF NOT EXISTS Idx_Type
			                          on Attributes (Type);
			                      """;
			command.ExecuteNonQuery();
		}
		finally
		{
			connection.Close();
		}
	}
}