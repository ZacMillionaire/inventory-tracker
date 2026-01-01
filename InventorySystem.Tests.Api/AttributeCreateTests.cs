using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Models;

namespace InventorySystem.Tests.Api;

public class AttributeCreateTests : IDisposable
{
	private readonly JsonSerializerOptions _jsonOptions;
	private readonly ApiWebApplicationFactory _apiWebApplicationFactory;

	public AttributeCreateTests()
	{
		_apiWebApplicationFactory = new ApiWebApplicationFactory();
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
		var client = _apiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "test";
			})
			.CreateClient();

		var response = await client.PostAsJsonAsync("/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		}, _jsonOptions);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var body = await response.Content.ReadFromJsonAsync<AttributeDto>(_jsonOptions);

		Assert.NotNull(body);
		Assert.NotEqual(Guid.Empty, body.Id);
		Assert.Equal("String Attribute", body.Name);
		Assert.Equal("string_attribute", body.KeyName);
		Assert.Equal(AttributeType.String, body.Type);
	}


	[Fact]
	public async Task POST_ForExistingName_ReturnsBadRequest()
	{
		var client = _apiWebApplicationFactory.CreateClient();
		var ctx = _apiWebApplicationFactory.Context;

		ctx.CreateEntity(new()
		{
			Name = "String Attribute",
			KeyName = "string_attribute",
			Type = AttributeType.String,
			Id = Guid.CreateVersion7(_apiWebApplicationFactory.TimeProvider.GetUtcNow())
		});


		var response2 = await client.PostAsJsonAsync("/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		}, _jsonOptions);

		Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
	}

	public void Dispose()
	{
		_apiWebApplicationFactory.Dispose();
	}
}