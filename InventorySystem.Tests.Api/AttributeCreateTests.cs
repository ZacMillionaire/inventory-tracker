using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Models;

namespace InventorySystem.Tests.Api;

public sealed class AttributeCreateTests : IDisposable
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

		var body = await ReadResponseJson<List<AttributeDto>>(response);
		Assert.NotNull(body);
		Assert.Empty(body);
	}

	[Fact]
	public async Task GET_Returns_Attributes()
	{
		var client = _apiWebApplicationFactory.CreateClient();
		var ctx = _apiWebApplicationFactory.Context;

		var now = _apiWebApplicationFactory.TimeProvider.GetUtcNow();

		// Create an attribute directly with explicit values
		// This sort of results in a test testing that the values we set are the values we set but that's the intent here
		ctx.Attributes.CreateEntity(new()
		{
			Name = "String Attribute",
			KeyName = "string_attribute",
			Type = AttributeType.String,
			Id = Guid.CreateVersion7(now),
			CreatedUtc = now
		});

		var response = await client.GetAsync("/attributes");
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<List<AttributeDto>>(response);
		Assert.NotNull(body);
		Assert.Single(body);
		var onlyResult = body[0];
		Assert.NotEqual(Guid.CreateVersion7(now), onlyResult.Id);
		Assert.Equal("String Attribute", onlyResult.Name);
		Assert.Equal("string_attribute", onlyResult.KeyName);
		Assert.Equal(AttributeType.String, onlyResult.Type);
		Assert.Equal(now, onlyResult.CreatedUtc);
		Assert.Null(onlyResult.UpdatedUtc);
	}

	[Fact]
	public async Task POST_creates_attribute()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);

		var client = _apiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "test";
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		var response = await client.PostAsJsonAsync("/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		}, _jsonOptions);
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<AttributeDto>(response);
		Assert.NotNull(body);
		Assert.NotEqual(Guid.CreateVersion7(timeProvider.GetUtcNow()), body.Id);
		Assert.Equal("String Attribute", body.Name);
		Assert.Equal("string_attribute", body.KeyName);
		Assert.Equal(AttributeType.String, body.Type);
		Assert.Equal(timeProvider.GetUtcNow(), body.CreatedUtc);
		Assert.Null(body.UpdatedUtc);
	}


	[Fact]
	public async Task POST_ForExistingName_ReturnsBadRequest()
	{
		var client = _apiWebApplicationFactory.CreateClient();
		var ctx = _apiWebApplicationFactory.Context;

		ctx.Attributes.CreateEntity(new()
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

	private Task<T?> ReadResponseJson<T>(HttpResponseMessage response)
	{
		return response.Content.ReadFromJsonAsync<T>(_jsonOptions);
	}

	public void Dispose()
	{
		_apiWebApplicationFactory.Dispose();
	}
}