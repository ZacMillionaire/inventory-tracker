//HintName: ItemApiRoutes.Api.g.cs
using Microsoft.AspNetCore.Builder;
using Tests.Complex.Namespace;
public static class WebApplicationExtensions
{
	extension(WebApplication app)
	{
		public void WithItemApiRoutes()
		{
			var apiGroup = app.MapGroup("/items");
			apiGroup.MapGet("/", ItemApiRoutes.GetItems)
				.WithName("GetItems");
			apiGroup.MapPost("Create", ItemApiRoutes.CreateItem)
				.WithName("CreateItem");
		}
	}
}