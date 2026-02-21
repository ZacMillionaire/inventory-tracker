using InventorySystem.Core;
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
	internal TimeProvider TimeProvider = TimeProvider.System;

	private readonly TestLoggerProvider _loggerProvider;
	private readonly ILogger _logger;

	public ApiWebApplicationFactory(ITestOutputHelper testOutputHelper)
	{
		_loggerProvider = new TestLoggerProvider(testOutputHelper);
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

		// Is be called after the `ConfigureServices` from the Startup
		// which allows you to overwrite the DI with mocked instances
		builder.ConfigureTestServices(services =>
		{
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