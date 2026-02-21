using System.Text.Json.Serialization;
using System.Threading.Tasks;
using InventorySystem.Core.Api;
using InventorySystem.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Apis;

[ApiGroup("attributes")]
public class AttributeApiRoutes
{
	[ApiGet("/")]
	public static Ok<List<AttributeDto>> GetAttribute(IAttributeRepository repo) => TypedResults.Ok(repo.Get());

	[ApiPost("Create")]
	public static async Task<Results<Ok<AttributeDto>, BadRequest>> CreateAttribute(IAttributeRepository repo, [FromBody] CreateAttributeDto dto) =>
		!await repo.AttributeExistsByName(dto.Name)
			? TypedResults.Ok(await repo.Create(dto))
			: TypedResults.BadRequest();
}

[JsonSerializable(typeof(AttributeDto))]
[JsonSerializable(typeof(CreateAttributeDto))]
internal partial class AttributeApiSerializerContext : JsonSerializerContext
{
}