using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Models;

namespace InventorySystem.Tests.Api.Items;

public sealed class ItemCreateTests : ApiTestBase
{
	[Fact]
	public async Task GET_Returns_NoItems()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);

		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "item-tests";
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		var response = await GetAsync(client, "/items");
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<List<ItemDto>>(response);
		Assert.NotNull(body);
	}

	[Fact]
	public async Task Get_CreatedItem_Should_MatchInList()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);

		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "item-tests";
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		var context = ApiWebApplicationFactory.Context;
		var createdItem = context.Items.CreateItem("Item 1", "Created First");

		var response = await GetAsync(client, "/items");
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<List<ItemDto>>(response);
		Assert.NotNull(body);
		Assert.Single(body);
		Assert.Equal(createdItem.Id, body[0].Id);
		Assert.Equal(createdItem.CreatedUtc, body[0].CreatedUtc);
	}

	[Fact]
	public async Task POST_CreateItem_WithoutAttributes()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);
		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "item-tests";
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		var response = await PostAsJsonAsync(client, "/items/create", new CreateItemRequestDto()
		{
			Name = "Item 1",
			Description = "A Description"
		});
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<ItemDto>(response);
		Assert.NotNull(body);
		Assert.NotEqual(Guid.Empty, body.Id);
		Assert.Equal("Item 1", body.Name);
		Assert.Equal("A Description", body.Description);
		Assert.Equal(timeProvider.GetUtcNow(), body.CreatedUtc);
		Assert.Null(body.UpdatedUtc);
	}

	[Fact]
	public async Task POST_CreateItemWithSameName_Should_OK()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);

		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "item-tests";
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		var context = ApiWebApplicationFactory.Context;

		var item = context.Items.CreateItem("Item 1", "Created First");

		var response = await PostAsJsonAsync(client, "/items/create", new CreateItemRequestDto()
		{
			Name = "Item 1",
			Description = "Created second"
		});
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<ItemDto>(response);
		Assert.NotNull(body);
		Assert.NotEqual(Guid.Empty, body.Id);
		Assert.Equal("Item 1", body.Name);
		Assert.Equal("Created second", body.Description);
		Assert.Equal(timeProvider.GetUtcNow(), body.CreatedUtc);
		Assert.Null(body.UpdatedUtc);
	}
	
	// TODO: get by id, delete by id, maybe change the api to POST to items to create instead of items/create?

	[Fact]
	public async Task POST_CreateItemWithAttributes_Should_OK()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);
		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "item-tests";
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		var response = await PostAsJsonAsync(client, "/items/create", new CreateItemRequestDto()
		{
			Name = "Item 1",
			Description = "A Description"
		});
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<ItemDto>(response);
		Assert.NotNull(body);
		Assert.NotEqual(Guid.Empty, body.Id);
		Assert.Equal("Item 1", body.Name);
		Assert.Equal("A Description", body.Description);
		Assert.Equal(timeProvider.GetUtcNow(), body.CreatedUtc);
		Assert.Null(body.UpdatedUtc);
	}
}

public class ApiTestBase : IDisposable
{
	private readonly JsonSerializerOptions _jsonOptions;
	protected readonly ApiWebApplicationFactory ApiWebApplicationFactory;

	public ApiTestBase()
	{
		ApiWebApplicationFactory = new ApiWebApplicationFactory();
		_jsonOptions = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true,
		};
		_jsonOptions.Converters.Add(new JsonStringEnumConverter<AttributeType>());
	}

	protected Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient client, string url, T body)
	{
		return client.PostAsJsonAsync(url, body, _jsonOptions);
	}

	protected Task<HttpResponseMessage> GetAsync(HttpClient client, string url)
	{
		return client.GetAsync(url);
	}

	protected Task<T?> ReadResponseJson<T>(HttpResponseMessage response)
	{
		return response.Content.ReadFromJsonAsync<T>(_jsonOptions);
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			ApiWebApplicationFactory.Dispose();
		}
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