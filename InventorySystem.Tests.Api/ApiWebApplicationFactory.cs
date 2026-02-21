using InventorySystem.Core;
using InventorySystem.Data;
using Marten;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Xunit.Abstractions;

namespace InventorySystem.Tests.Api;

public class ApiWebApplicationFactory : WebApplicationFactory<InventorySystemApi>
{
	private readonly ITestOutputHelper _testOutputHelper;
	private DbContextHelper? _context;
	private readonly string _generatedDatabaseFolder = "./GeneratedTestDatabases";

	internal DatabaseContext Context => _context?.GetContext ?? throw new Exception("Context has not yet been created. Call CreateClient() first.");

	internal string? DatabaseName;
	internal TimeProvider TimeProvider = TimeProvider.System;

	private TestLoggerProvider _loggerProvider;
	private ILogger _logger;

	public ApiWebApplicationFactory(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
		_loggerProvider = new TestLoggerProvider(_testOutputHelper);
		_logger = _loggerProvider.CreateLogger("ApiWebApplicationFactory");
	}

	internal ApiWebApplicationFactory Configure(Action<ApiWebApplicationFactory> config)
	{
		config(this);
		return this;
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureLogging(loggingBuilder =>
		{
			loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => _loggerProvider);
		});

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

	public override ValueTask DisposeAsync()
	{
		_logger.LogInformation("Cleaning document store");
		var store = Services.GetRequiredService<IDocumentStore>();
		// Ensure our database is clean
		store.Advanced.Clean.DeleteAllDocumentsAsync().Wait();
		_logger.LogInformation("Done");
		return base.DisposeAsync();
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