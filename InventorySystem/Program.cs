using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InventorySystem;

// Name differs from file name for clarity in tests, but consistency with standard project conventions
public class InventorySystemApi
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateSlimBuilder(args);

		builder.Services.AddSingleton(TimeProvider.System);

		builder.Services.ConfigureHttpJsonOptions(options =>
		{
			options.SerializerOptions.TypeInfoResolverChain.Add(AttributeApiSerializerContext.Default);
			options.SerializerOptions.TypeInfoResolverChain.Add(ItemApiSerializerContext.Default);
			options.SerializerOptions.PropertyNameCaseInsensitive = true;
			options.SerializerOptions.Converters.Add(new JsonStringEnumConverter<AttributeType>());
		});

		builder.Services.AddSingleton(new DatabaseContext("Data Source=:memory:"));

		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		builder.Services.AddSingleton<ItemRepository>();
		builder.Services.AddSingleton<AttributeRepository>();

		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
		}

		app.WithItemApiRoutes();
		app.WithAttributeApiRoutes();

		// Todo[] sampleTodos =
		// [
		// 	new(1, "Walk the dog"),
		// 	new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
		// 	new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
		// 	new(4, "Clean the bathroom"),
		// 	new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
		// ];
		//
		// var todosApi = app.MapGroup("/todos");
		// todosApi.MapGet("/", () => sampleTodos)
		// 	.WithName("GetTodos");
		//
		// todosApi.MapGet("/{id}", Results<Ok<Todo>, NotFound> (int id) =>
		// 		sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
		// 			? TypedResults.Ok(todo)
		// 			: TypedResults.NotFound())
		// 	.WithName("GetTodoById");

		app.Run();
	}
}

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

// [JsonSerializable(typeof(Todo[]))]
// [JsonSerializable(typeof(ItemDto[]))]
// [JsonSerializable(typeof(CreateItemRequestDto[]))]
// [JsonSerializable(typeof(AttributeDto))]
// [JsonSerializable(typeof(CreateAttributeDto))]
// // [JsonSerializable(typeof(AttributeValueDto[]))]
// internal partial class AppJsonSerializerContext : JsonSerializerContext
// {
// }