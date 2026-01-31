using Microsoft.Data.Sqlite;

namespace InventorySystem.Data.Migrations;

public class StringAttributeValueTableMigration
{
	public static void Up(SqliteConnection connection)
	{
		try
		{
			connection.Open();
			using var command = connection.CreateCommand();
			command.CommandText = """
			                      create table StringAttributeValue
			                      (
			                          AttributeId TEXT not null
			                              constraint StringAttributeValue_Attributes_Id_fk
			                                  references Attributes,
			                          ItemId      TEXT not null
			                              constraint StringAttributeValue_Items_Id_fk
			                                  references Items,
			                          Value       TEXT not null,
			                          constraint StringAttributeValue_pk
			                              unique (AttributeId, ItemId)
			                      );
			                      """;
			command.ExecuteNonQuery();
		}
		finally
		{
			connection.Close();
		}
	}
}