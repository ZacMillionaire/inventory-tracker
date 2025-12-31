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

public class AttributeCreateTests : IClassFixture<WebApplicationFactory<InventorySystemApi>>
{
	readonly HttpClient _client;
	private readonly JsonSerializerOptions _jsonOptions;

	public AttributeCreateTests(WebApplicationFactory<InventorySystemApi> application)
	{
		_client = application.CreateClient();

		_jsonOptions = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true,
		};
		_jsonOptions.Converters.Add(new JsonStringEnumConverter<AttributeType>());
	}

	[Fact]
	public async Task GET_Returns_NoAttributes()
	{
		var response = await _client.GetAsync("/attributes");
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var body = await response.Content.ReadFromJsonAsync<List<AttributeDto>>();
		Assert.NotNull(body);
		Assert.Empty(body);
	}

	[Fact]
	public async Task POST_creates_attribute()
	{
		var response = await _client.PostAsJsonAsync("/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		}, _jsonOptions);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var body = await response.Content.ReadFromJsonAsync<AttributeDto>(_jsonOptions);

		Assert.NotNull(body);
		Assert.Equal(1, body.Id);
		Assert.Equal("String Attribute", body.Name);
		Assert.Equal("string_attribute", body.KeyName);
		Assert.Equal(AttributeType.String, body.Type);
	}


	[Fact]
	public async Task POST_ForExistingName_ReturnsBadRequest()
	{
		// var ctx = new DatabaseContext("Data Source=:memory:");
		var ctx = new DatabaseContext("Data Source=test.db");
		
		// ctx.Seed(conn =>
		// {
		// });
		
		var a = new ApiWebApplicationFactory(ctx);

		var client = a.CreateClient();

		var response = await client.PostAsJsonAsync("/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		}, _jsonOptions);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var body = await response.Content.ReadFromJsonAsync<AttributeDto>(_jsonOptions);

		Assert.NotNull(body);
		Assert.Equal(1, body.Id);
		Assert.Equal("String Attribute", body.Name);
		Assert.Equal("string_attribute", body.KeyName);
		Assert.Equal(AttributeType.String, body.Type);

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
	private readonly DatabaseContext _context;

	public ApiWebApplicationFactory(DatabaseContext context)
	{
		_context = context;
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		// Is be called after the `ConfigureServices` from the Startup
		// which allows you to overwrite the DI with mocked instances
		builder.ConfigureTestServices(services =>
		{
			services.AddSingleton(_context);
		});
	}
}