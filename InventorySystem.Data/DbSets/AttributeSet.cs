using System.Data;
using InventorySystem.Data.Entities;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Models;
using Microsoft.Data.Sqlite;

namespace InventorySystem.Data.DbSets;

public class AttributeSet
{
	private readonly SqliteConnection _connection;

	internal AttributeSet(SqliteConnection connection)
	{
		_connection = connection;
	}

	internal EntityAttribute CreateEntity(EntityAttribute entityAttribute)
	{
		return RunInConnection(() =>
		{
			using var insertCommand = _connection.CreateCommand();

			insertCommand.CommandText = """
			                            INSERT INTO Attributes (Id, Name, KeyName, Type)
			                            VALUES ($id, $name, $keyName, $type) 
			                            Returning RowId
			                            """;

			insertCommand.Parameters.AddRange([
				new SqliteParameter("id", entityAttribute.Id.ToString()),
				new SqliteParameter("name", entityAttribute.Name),
				new SqliteParameter("keyName", entityAttribute.KeyName),
				new SqliteParameter("type", entityAttribute.Type),
			]);

			var insertedId = insertCommand.ExecuteScalar();

			return entityAttribute;
		});
	}

	internal List<AttributeDto> GetAttributes(int page = 1, int perPage = 25)
	{
		return RunInConnection(() =>
		{
			using var queryCommand = _connection.CreateCommand();
			queryCommand.CommandText = """
			                           SELECT Id, Name, KeyName, Type 
			                           FROM Attributes
			                           LIMIT $perPage
			                           OFFSET $page
			                           """;
			queryCommand.Parameters.AddRange([
				new SqliteParameter("perPage", perPage),
				new SqliteParameter("page", page * perPage - perPage)
			]);


			var attributes = new List<AttributeDto>();
			var attributeReader = queryCommand.ExecuteReader();

			while (attributeReader.Read())
			{
				var row = new AttributeDto()
				{
					Id = attributeReader.GetGuid("Id"),
					Name = attributeReader.GetString("Name"),
					KeyName = attributeReader.GetString("KeyName"),
					Type = Enum.Parse<AttributeType>(attributeReader.GetString("Type"))
				};

				attributes.Add(row);
			}

			return attributes;
		});
	}

	public bool AttributeExistsByName(string attributeName)
	{
		return RunInConnection(() =>
		{
			using var queryCommand = _connection.CreateCommand();
			queryCommand.CommandText = """
			                           SELECT 1 
			                           FROM Attributes
			                           WHERE Name = $name
			                           """;
			queryCommand.Parameters.AddRange([
				new SqliteParameter("name", attributeName),
			]);

			return queryCommand.ExecuteScalar() != null;
		});
	}

	protected T RunInConnection<T>(Func<T> func)
	{
		try
		{
			_connection.Open();
			return func();
		}
		finally
		{
			_connection.Close();
		}
	}
}