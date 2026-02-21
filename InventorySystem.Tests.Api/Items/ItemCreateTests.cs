using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventorySystem.Data;
using InventorySystem.Data.Entities;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace InventorySystem.Tests.Api.Items;

public sealed class ItemCreateTests : ApiTestBase
{
	public ItemCreateTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

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
		Assert.Empty(body);
	}

	[Fact]
	public async Task Get_CreatedItem_Should_MatchInList()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);

		var expectedName = "Item 1";
		var expectedDescription = "Created First";

		var createdItem = await CreateItem(expectedName, expectedDescription, timeProvider);

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
		Assert.Single(body);
		Assert.Equal(createdItem.Id, body[0].Id);
		Assert.Equal(createdItem.CreatedUtc, body[0].CreatedUtc);
		Assert.Equal(expectedName, body[0].Name);
		Assert.Equal(expectedDescription, body[0].Description);
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
		Assert.NotEqual(Guid.CreateVersion7(timeProvider.GetUtcNow()), body.Id);
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

		var item = await CreateItem("Item 1", "Created First", timeProvider);

		var response = await PostAsJsonAsync(client, "/items/create", new CreateItemRequestDto()
		{
			Name = "Item 1",
			Description = "Created second"
		});
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<ItemDto>(response);
		Assert.NotNull(body);
		Assert.NotEqual(Guid.Empty, body.Id);
		Assert.Equal(item.Name, body.Name);
		Assert.Equal("Created second", body.Description);
		Assert.NotEqual(item.Description, body.Description);
		Assert.Equal(timeProvider.GetUtcNow(), body.CreatedUtc);
		Assert.Null(body.UpdatedUtc);

		var itemsListResponse = await GetAsync(client, "/items");
		Assert.Equal(HttpStatusCode.OK, itemsListResponse.StatusCode);

		var itemsListBody = await ReadResponseJson<List<ItemDto>>(itemsListResponse);
		Assert.NotNull(itemsListBody);
		Assert.Equal(2, itemsListBody.Count);
	}

	// TODO: get by id, delete by id, maybe change the api to POST to items to create instead of items/create?

	// TODO implement attributes first and redo this test
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

	private async Task<Item> CreateItem(string name, string description, TimeProvider timeProvider)
	{
		await using var scope = ApiWebApplicationFactory.Services.CreateAsyncScope();
		ItemRepository itemRepository = (ItemRepository)scope.ServiceProvider.GetRequiredService<ItemRepository>();

		var createdItem = await itemRepository.CreateAsyncImpl(new Item()
		{
			Name = name,
			Description = description,
			CreatedUtc = timeProvider.GetUtcNow(),
			Id = Guid.CreateVersion7(timeProvider.GetUtcNow())
		});

		return createdItem;
	}
}

public class ApiTestBase : IDisposable
{
	private readonly JsonSerializerOptions _jsonOptions;
	protected readonly ApiWebApplicationFactory ApiWebApplicationFactory;

	protected ApiTestBase(ITestOutputHelper testOutputHelper)
	{
		ApiWebApplicationFactory = new ApiWebApplicationFactory(testOutputHelper);
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