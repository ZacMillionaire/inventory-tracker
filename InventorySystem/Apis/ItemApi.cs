using System.Text.Json.Serialization;
using InventorySystem.Core.Api;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Apis;

[ApiGroup("items")]
public class ItemApiRoutes
{
	[ApiGet("/")]
	public static List<ItemDto> GetItems(ItemRepository repo) => repo.Get();

	[ApiPost("Create")]
	public static ItemDto CreateItem(ItemRepository repo, [FromBody] CreateItemRequestDto dto) => repo.Create(dto);
}

[JsonSerializable(typeof(ItemDto))]
[JsonSerializable(typeof(CreateItemRequestDto))]
internal partial class ItemApiSerializerContext : JsonSerializerContext
{
}