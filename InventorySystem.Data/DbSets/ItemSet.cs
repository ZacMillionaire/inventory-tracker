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
				CreatedUtc = now
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
			// TODO: create attribute if it doesn't?

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
}