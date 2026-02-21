using InventorySystem.Core;
using InventorySystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace InventorySystem.Tests.Api;

public class ApiWebApplicationFactory : WebApplicationFactory<InventorySystemApi>
{
	private DbContextHelper? _context;
	private readonly string _generatedDatabaseFolder = "./GeneratedTestDatabases";

	internal DatabaseContext Context => _context?.GetContext ?? throw new Exception("Context has not yet been created. Call CreateClient() first.");

	internal string? DatabaseName;
	internal TimeProvider TimeProvider = TimeProvider.System;

	internal ApiWebApplicationFactory Configure(Action<ApiWebApplicationFactory> config)
	{
		config(this);
		return this;
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		Directory.CreateDirectory(_generatedDatabaseFolder);

		var connectionString = $"Data Source={_generatedDatabaseFolder}/{DatabaseName ?? Guid.NewGuid().ToString()}.db";

		// set up the connection string for postgres, settings from ./Containers/docker-compose.yml
		var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder()
		{
			Database = "postgres",
			Username = "postgres",
			Password = "postgres",
			Host = "localhost",
			Port = 5433
		};

		Environment.SetEnvironmentVariable($"ConnectionStrings__{EnvironmentKeys.PostgresDbEnvironmentKey}", npgsqlConnectionStringBuilder.ToString());
		
		_context ??= new DbContextHelper(connectionString, TimeProvider);

		// Is be called after the `ConfigureServices` from the Startup
		// which allows you to overwrite the DI with mocked instances
		builder.ConfigureTestServices(services =>
		{
			services.AddSingleton(_context.GetContext);
			services.AddSingleton(TimeProvider);
		});
	}

	protected override void Dispose(bool disposing)
	{
		_context?.Dispose();
		base.Dispose(disposing);
	}
}

internal class TestTimeProvider : TimeProvider
{
	private readonly DateTimeOffset _time;

	public override DateTimeOffset GetUtcNow() => _time.UtcDateTime;

	public TestTimeProvider(DateTimeOffset startingTime)
	{
		_time = startingTime;
	}
}