using System;
using System.Text.Json.Serialization;
using InventorySystem.Core;
using InventorySystem.Data.Attributes;
using InventorySystem.Data.Interfaces;
using JasperFx;
using JasperFx.CodeGeneration;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

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

		// Required for UseNpgsqlDataSource below
		builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString(EnvironmentKeys.PostgresDbEnvironmentKey)!);

		builder.Services.AddMarten(options =>
			{
				// If you want the Marten controlled PostgreSQL objects
				// in a different schema other than "public"
				options.DatabaseSchemaName = "InventorySystem";
			})
			// This is recommended in new development projects
			.UseLightweightSessions()
			.UseNpgsqlDataSource();

		builder.Services.CritterStackDefaults(options =>
		{
			// Ensure that types are always generated for development mode
			options.Development.GeneratedCodeMode = TypeLoadMode.Auto;
			options.Development.ResourceAutoCreate = AutoCreate.All;
			
			options.Production.GeneratedCodeMode = TypeLoadMode.Static;
			options.Production.ResourceAutoCreate = AutoCreate.None;

			// If we can help it we don't want to have long start ups for tests if we already have generated types.
			// In saying this though, if generated types aren't up to date tests will still run,
			// they'll just be a bit slower
			if (builder.Environment.EnvironmentName == "Test")
			{
				options.Development.GeneratedCodeMode = TypeLoadMode.Static;
				options.Development.ResourceAutoCreate = AutoCreate.None;
			}
		});

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
		services.AddScoped<ItemRepository>();
		services.AddScoped<IAttributeRepository, AttributeRepository>();
		services.AddScoped<AttributeValueRepository>();
	}

	private static void MapApiRoutes(WebApplication app)
	{
		app.WithItemApiRoutes();
		app.WithAttributeApiRoutes();
	}
}