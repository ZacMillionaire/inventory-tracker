using System.Text.Json.Serialization;
using System.Threading.Tasks;
using InventorySystem.Core.Api;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Apis;

[ApiGroup("items")]
public class ItemApiRoutes
{
	// TODO: wrap in a TypedResults
	[ApiGet("/")]
	public static async Task<List<ItemDto>> GetItems(ItemRepository repo) => await repo.Get();

	[ApiPost("Create")]
	public static async Task<ItemDto> CreateItem(ItemRepository repo, [FromBody] CreateItemRequestDto dto) => await repo.Create(dto);
}

[JsonSerializable(typeof(ItemDto))]
[JsonSerializable(typeof(CreateItemRequestDto))]
internal partial class ItemApiSerializerContext : JsonSerializerContext
{
}