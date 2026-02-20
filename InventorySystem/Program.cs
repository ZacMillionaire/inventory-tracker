using System;
using System.Text.Json.Serialization;
using InventorySystem.Core;
using InventorySystem.Data.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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

		// TODO: make this not be in memory
		builder.Services.AddSingleton(new DatabaseContext("Data Source=:memory:"));
		var a = builder.Configuration.GetConnectionString(EnvironmentKeys.PostgresDbEnvironmentKey);

		builder.AddServiceDefaults();

		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		AddRepositories(builder.Services);

		var app = builder.Build();

		app.MapDefaultEndpoints();

		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
		}

		MapApiRoutes(app);

		app.Run();
	}

	private static void AddRepositories(IServiceCollection services)
	{
		services.AddSingleton<ItemRepository>();
		services.AddSingleton<AttributeRepository>();
		services.AddSingleton<AttributeValueRepository>();
	}

	private static void MapApiRoutes(WebApplication app)
	{
		app.WithItemApiRoutes();
		app.WithAttributeApiRoutes();
	}
}