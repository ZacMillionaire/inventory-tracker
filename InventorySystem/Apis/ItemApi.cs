using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Apis;

public class ItemApi
{
	public static void AddItemApiRoutes(WebApplication app)
	{
		var apiGroup = app.MapGroup("/items");
		apiGroup.MapGet("/", (ItemRepository repo) =>
			{
				repo.Get();
			})
			.WithName("GetItems");

		apiGroup.MapPost("/Create", (ItemRepository repo, [FromBody] CreateItemRequestDto dto) =>
				repo.Create(dto)
			)
			.WithName("CreateItem");
	}
}

[JsonSerializable(typeof(CreateItemRequestDto))]
internal partial class ItemApiSerializerContext : JsonSerializerContext
{
}