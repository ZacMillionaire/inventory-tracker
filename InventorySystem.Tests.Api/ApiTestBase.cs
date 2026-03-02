using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventorySystem.Data.Enums;

namespace InventorySystem.Tests.Api;

public class ApiTestBase : IDisposable
{
	private readonly JsonSerializerOptions _jsonOptions;
	protected readonly ApiWebApplicationFactory ApiWebApplicationFactory;

	protected ApiTestBase()
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