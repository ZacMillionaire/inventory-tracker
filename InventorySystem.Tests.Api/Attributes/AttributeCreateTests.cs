using System.Net;
using InventorySystem.Data;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Interfaces;
using InventorySystem.Data.Models;
using Microsoft.Extensions.DependencyInjection;

namespace InventorySystem.Tests.Api.Attributes;

public sealed class AttributeCreateTests : ApiTestBase
{
	[Fact]
	public async Task GET_Returns_NoAttributes()
	{
		var client = ApiWebApplicationFactory.CreateClient();

		var response = await GetAsync(client, "/attributes");
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<List<AttributeDto>>(response);
		Assert.NotNull(body);
		Assert.Empty(body);
	}

	[Fact]
	public async Task GET_Returns_Attributes()
	{
		var now = ApiWebApplicationFactory.TimeProvider.GetUtcNow();

		await using (var scope = ApiWebApplicationFactory.Services.CreateAsyncScope())
		{
			AttributeRepository attributeRepository = (AttributeRepository)scope.ServiceProvider.GetRequiredService<IAttributeRepository>();
			// Create an attribute directly with explicit values
			// This sort of results in a test testing that the values we set are the values we set but that's the intent here
			await attributeRepository.CreateAsyncImpl(new()
			{
				Name = "String Attribute",
				KeyName = "string_attribute",
				Type = AttributeType.String,
				Id = Guid.CreateVersion7(ApiWebApplicationFactory.TimeProvider.GetUtcNow()),
				CreatedUtc = now
			});
		}

		var client = ApiWebApplicationFactory.CreateClient();

		var response = await GetAsync(client, "/attributes");
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

		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		var response = await PostAsJsonAsync(client, "/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		});

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
		// Directly create an entity to conflict against
		await using (var scope = ApiWebApplicationFactory.Services.CreateAsyncScope())
		{
			AttributeRepository attributeRepository = (AttributeRepository)scope.ServiceProvider.GetRequiredService<IAttributeRepository>();

			await attributeRepository.CreateAsyncImpl(new()
			{
				Name = "String Attribute",
				KeyName = "string_attribute",
				Type = AttributeType.String,
				Id = Guid.CreateVersion7(ApiWebApplicationFactory.TimeProvider.GetUtcNow())
			});
		}

		var client = ApiWebApplicationFactory.CreateClient();
		var response2 = await PostAsJsonAsync(client, "/attributes/create", new CreateAttributeDto()
		{
			Name = "String Attribute",
			Type = AttributeType.String
		});

		Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
	}
}