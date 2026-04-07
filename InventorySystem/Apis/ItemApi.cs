using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using InventorySystem.Core.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Apis;

[ApiGroup("items")]
public class ItemApiRoutes
{
	// TODO: wrap in a TypedResults
	[ApiGet("/")]
	public static async Task<List<ItemDto>> GetItems(ItemRepository repo) => await repo.Get();

	[ApiPost("Create")]
	public static async Task<Results<Ok<ItemDto>, BadRequest<string>>> CreateItem(ItemRepository repo, [FromBody] CreateItemRequestDto dto)
	{
		try
		{
			return TypedResults.Ok(await repo.Create(dto));
		}
		catch (Exception ex)
		{
			return TypedResults.BadRequest(ex.Message);
		}
	}
}

[JsonSerializable(typeof(ItemDto))]
[JsonSerializable(typeof(CreateItemRequestDto))]
internal partial class ItemApiSerializerContext : JsonSerializerContext
{
}