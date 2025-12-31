using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Apis;

public class AttributeApi
{
	public static void AddAttributeApiRoutes(WebApplication app)
	{
		var apiGroup = app.MapGroup("/attributes");
		apiGroup.MapGet("/", Ok<List<AttributeDto>> (AttributeRepository repo) =>
				TypedResults.Ok(repo.Get())
			)
			.WithName("GetAttribute");

		apiGroup.MapPost("/Create", Results<Ok<AttributeDto>, BadRequest> (AttributeRepository repo, [FromBody] CreateAttributeDto dto) =>
				!repo.AttributeExistsByName(dto.Name)
					? TypedResults.Ok(repo.Create(dto))
					: TypedResults.BadRequest()
			)
			.WithName("CreateAttribute");
	}
}

[JsonSerializable(typeof(AttributeDto))]
[JsonSerializable(typeof(CreateAttributeDto))]
internal partial class AttributeApiSerializerContext : JsonSerializerContext
{
}