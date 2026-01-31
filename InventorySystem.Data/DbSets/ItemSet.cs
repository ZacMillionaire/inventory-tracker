using System.Data;
using InventorySystem.Data.Entities;
using InventorySystem.Data.Models;
using Microsoft.Data.Sqlite;

namespace InventorySystem.Data.DbSets;

public class ItemSet
{
	private readonly SqliteConnection _connection;
	private readonly TimeProvider _timeProvider;

	public ItemSet(SqliteConnection connection, TimeProvider? timeProvider = null)
	{
		_connection = connection;
		_timeProvider = timeProvider ?? TimeProvider.System;
	}

	/// <summary>
	/// Creates a new item with the given name and description, linking to any attributes given.
	/// <remarks>
	///	If an attribute does not exist, no item will be created and an exception will be thrown.
	/// </remarks>
	/// </summary>
	/// <param name="name"></param>
	/// <param name="description"></param>
	/// <param name="attributes"></param>
	/// <returns></returns>
	internal Item CreateItem(string name, string? description = null, List<AttributeDto>? attributes = null)
	{
		return RunInConnection(() =>
		{
			var now = _timeProvider.GetUtcNow();
			var newItem = new Item()
			{
				Id = Guid.CreateVersion7(now),
				Name = name,
				Description = description,
				CreatedUtc = now,
			};

			using var insertCommand = _connection.CreateCommand();

			insertCommand.CommandText = """
			                            INSERT INTO Items (Id, Name, Description, CreatedUtc)
			                            VALUES ($id, $name, $description, $createdUtc)
			                            """;

			insertCommand.Parameters.AddRange([
				new SqliteParameter("id", newItem.Id.ToString()),
				new SqliteParameter("name", newItem.Name),
				new SqliteParameter("description", newItem.Description),
				new SqliteParameter("createdUtc", now.Ticks),
			]);

			insertCommand.ExecuteScalar();

			// TODO: link attributes by Id if they exist

			return newItem;
		});
	}

	private T RunInConnection<T>(Func<T> func)
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

	public List<ItemDto> GetItems(int page = 1, int perPage = 25)
	{
		return RunInConnection(() =>
		{
			using var queryCommand = _connection.CreateCommand();
			queryCommand.CommandText = """
			                           SELECT Id, Name, Description, CreatedUtc, UpdatedUtc 
			                           FROM Items
			                           LIMIT $perPage
			                           OFFSET $page
			                           """;
			queryCommand.Parameters.AddRange([
				new SqliteParameter("perPage", perPage),
				new SqliteParameter("page", page * perPage - perPage)
			]);


			var items = new List<ItemDto>();
			var attributeReader = queryCommand.ExecuteReader();

			while (attributeReader.Read())
			{
				var row = new ItemDto()
				{
					Id = attributeReader.GetGuid("Id"),
					Name = attributeReader.GetString("Name"),
					Description = attributeReader.GetString("Description"),
					CreatedUtc = new DateTimeOffset(attributeReader.GetInt64("CreatedUtc"), TimeSpan.Zero),
					UpdatedUtc = attributeReader.IsDBNull("UpdatedUtc")
						? null
						: new DateTimeOffset(attributeReader.GetInt64("UpdatedUtc"), TimeSpan.Zero),
				};

				items.Add(row);
			}

			return items;
		});
	}
}