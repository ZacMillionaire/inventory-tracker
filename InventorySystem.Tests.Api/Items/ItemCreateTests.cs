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
	public async Task POST_CreateItem_WithoutAttributes()
	{
		var client = ApiWebApplicationFactory
			.Configure(config =>
			{
				config.DatabaseName = "item-tests";
			})
			.CreateClient();

		var response = await PostAsJsonAsync(client, "/items/create", new CreateItemRequestDto()
		{
			Name = "Item 1",
			Description = "A Description"
		});
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		var body = await ReadResponseJson<AttributeDto>(response);
		Assert.NotNull(body);
		Assert.NotEqual(Guid.Empty, body.Id);
		Assert.Equal("String Attribute", body.Name);
		Assert.Equal("string_attribute", body.KeyName);
		Assert.Equal(AttributeType.String, body.Type);
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