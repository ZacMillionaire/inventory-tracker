using System.Net;
using System.Net.Http.Json;
using InventorySystem.Data;
using InventorySystem.Data.Entities;
using InventorySystem.Data.Models;
using Microsoft.Extensions.DependencyInjection;

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

	[Fact]
	public async Task POST_CreateItemWithSameName_FirstDistinct_Should_BadRequest()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);

		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.TimeProvider = timeProvider;
			})
			.CreateClient();

		await CreateItem("Item 1", "Created First", timeProvider, true);

		// TODO: get by id, delete by id, maybe change the api to POST to items to create instead of items/create?
		var response = await PostAsJsonAsync(client, "/items/create", new CreateItemRequestDto()
		{
			Name = "Item 1",
			Description = "Created second"
		});
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		// TypedResult turns the response body into json, and currently I just pass the exception message directly to it
		// TODO: I should make the error response a class
		Assert.Equal("Item already exists as distinct", await response.Content.ReadFromJsonAsync<string>(TestContext.Current.CancellationToken));
	}


	// TODO implement attributes first and redo this test
	public async Task POST_CreateItemWithAttributes_Should_OK()
	{
		var timeProvider = new TestTimeProvider(DateTimeOffset.Now);
		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
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

	private async Task<Item> CreateItem(string name, string description, TimeProvider timeProvider, bool asDistinct = false)
	{
		await using var scope = ApiWebApplicationFactory.Services.CreateAsyncScope();
		var itemRepository = scope.ServiceProvider.GetRequiredService<ItemRepository>();

		var createdItem = await itemRepository.CreateAsyncImpl(new Item()
		{
			Name = name,
			Description = description,
			CreatedUtc = timeProvider.GetUtcNow(),
			Distinct = asDistinct,
			Id = Guid.CreateVersion7(timeProvider.GetUtcNow()),
			// TODO: make this property (and the normalise name method) internal and private respectively
			NormalisedName = itemRepository.NormaliseItemName(name),
		});

		return createdItem;
	}
}