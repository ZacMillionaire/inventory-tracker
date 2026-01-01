using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventorySystem.Data;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace InventorySystem.Tests.Api;

public class AttributeCreateTests : IClassFixture<ApiWebApplicationFactory>
{
	private readonly JsonSerializerOptions _jsonOptions;
	private readonly ApiWebApplicationFactory _apiWebApplicationFactory;

	public AttributeCreateTests(ApiWebApplicationFactory application)
	{
		_apiWebApplicationFactory = application;
		_jsonOptions = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true,
		};
		_jsonOptions.Converters.Add(new JsonStringEnumConverter<AttributeType>());
	}

	[Fact]
	public async Task GET_Returns_NoAttributes()
	{
		var client = _apiWebApplicationFactory.CreateClient();
		var response = await client.GetAsync("/attributes");
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var body = await response.Content.ReadFromJsonAsync<List<AttributeDto>>();
		Assert.NotNull(body);
		Assert.Empty(body);
	}

	[Fact]
	public async Task POST_creates_attribute()
	{
		var client = _apiWebApplicationFactory.CreateClient();
		var response = await client.PostAsJsonAsync("/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		}, _jsonOptions);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var body = await response.Content.ReadFromJsonAsync<AttributeDto>(_jsonOptions);

		Assert.NotNull(body);
		// Assert.Equal(1, body.Id);
		Assert.Equal("String Attribute", body.Name);
		Assert.Equal("string_attribute", body.KeyName);
		Assert.Equal(AttributeType.String, body.Type);
	}


	[Fact]
	public async Task POST_ForExistingName_ReturnsBadRequest()
	{
		using var helper = new DbContextHelper("Data Source=test.db");
		var ctx = helper.GetContext;
		var apiWebApplicationFactory = new ApiWebApplicationFactory(ctx);

		ctx.CreateEntity(new()
		{
			Name = "String Attribute",
			KeyName = "string_attribute",
			Type = AttributeType.String,
			Id = Guid.CreateVersion7(apiWebApplicationFactory.TimeProvider.GetUtcNow())
		});

		var client = apiWebApplicationFactory.CreateClient();

		var response2 = await client.PostAsJsonAsync("/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		}, _jsonOptions);

		Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
	}
}

public class ApiWebApplicationFactory : WebApplicationFactory<InventorySystemApi>
{
	private DatabaseContext? _context;
	private readonly string _connectionString;
	internal TimeProvider TimeProvider = TimeProvider.System;

	/// <summary>
	/// Uses an in memory Sqlite database context
	/// </summary>
	public ApiWebApplicationFactory()
	{
		_connectionString = "Data Source=:memory:";
		// _context = new DatabaseContext("Data Source=:memory:");
	}

	internal ApiWebApplicationFactory(string connectionString)
	{
		_connectionString = connectionString;
	}

	internal ApiWebApplicationFactory(DatabaseContext context)
	{
		_context = context;
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		// If the direct context constructor wasn't used, create a context from the connection string
		_context ??= new DatabaseContext(_connectionString);

		// Is be called after the `ConfigureServices` from the Startup
		// which allows you to overwrite the DI with mocked instances
		builder.ConfigureTestServices(services =>
		{
			services.AddSingleton(_context);
		});
	}
}