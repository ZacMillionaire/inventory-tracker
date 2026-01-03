using InventorySystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

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