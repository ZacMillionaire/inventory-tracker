using System.Text.Json.Serialization;
using InventorySystem.Core.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Apis;

[ApiGroup("attributes")]
public class AttributeApiRoutes
{
	[ApiGet("/")]
	public static Ok<List<AttributeDto>> GetAttribute(AttributeRepository repo) => TypedResults.Ok(repo.Get());

	[ApiPost("Create")]
	public static Results<Ok<AttributeDto>, BadRequest> CreateAttribute(AttributeRepository repo, [FromBody] CreateAttributeDto dto) =>
		!repo.AttributeExistsByName(dto.Name)
			? TypedResults.Ok(repo.Create(dto))
			: TypedResults.BadRequest();
}

[JsonSerializable(typeof(AttributeDto))]
[JsonSerializable(typeof(CreateAttributeDto))]
internal partial class AttributeApiSerializerContext : JsonSerializerContext
{
}