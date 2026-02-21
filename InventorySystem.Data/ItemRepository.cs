using System.Text.Json.Serialization;
using InventorySystem.Data.Entities;
using InventorySystem.Data.Models;
using Marten;

namespace InventorySystem.Data;

public class ItemRepository
{
	private readonly DatabaseContext _database;
	private readonly IDocumentSession _documentSession;
	private readonly TimeProvider _timeProvider;

	public ItemRepository(TimeProvider timeProvider, DatabaseContext dataStorge, IDocumentSession documentSession)
	{
		_timeProvider = timeProvider;
		_database = dataStorge;
		_documentSession = documentSession;
	}

	public async Task<ItemDto> Create(CreateItemRequestDto item)
	{
		// var itemDto = new Item()
		// {
		// 	Name = newItem.Name,
		// 	Description = newItem.Description,
		// 	Attributes = AttributeValueDtoToAttribute(newItem.AttributeValues)
		// };

		// TODO: link attributes and create values
		var newItem = await CreateAsyncImpl(new Item()
		{
			Name = item.Name,
			Description = item.Description,
			CreatedUtc = _timeProvider.GetUtcNow(),
			Id = Guid.CreateVersion7(_timeProvider.GetUtcNow())
		});

		return ToDto(newItem);
	}

	internal async Task<Item> CreateAsyncImpl(Item item)
	{
		_documentSession.Store(item);

		await _documentSession.SaveChangesAsync();

		return item;
	}

	private List<EntityAttribute> AttributeValueDtoToAttribute(List<AttributeValueDto> attributeValueDto)
	{
		return [];
		// return attributeValueDto.Select(y => _attributes.FirstOrDefault(x => x.Id == y.AttributeId))
		// 	.Where(x => x != null)
		// 	.ToList();
	}

	public async Task<List<ItemDto>> Get()
	{
		var attributes = await _documentSession.Query<Item>()
			.ToListAsync();

		return attributes.Select(ToDto).ToList();
		// return _items.Select(x => new ItemDto()
		// 	{
		// 		Name = x.Name,
		// 		Description = x.Description,
		// 		Id = x.Id,
		// 		Attributes = x.Attributes.Select(y => new AttributeDto()
		// 			{
		// 				Name = y.Name,
		// 				KeyName = y.KeyName,
		// 				Id = y.Id,
		// 				// IntValue = y.IntValue,
		// 				// DoubleValue = y.DoubleValue,
		// 				// StringValue = y.StringValue,
		// 				Type = y.Type
		// 			})
		// 			.ToList()
		// 	})
		// 	.ToList();
	}

	private ItemDto ToDto(Item item)
	{
		return new ItemDto()
		{
			Id = item.Id,
			Name = item.Name,
			Description = item.Description,
			Attributes = [], // TODO: consolidate the making of Dtos to the attribute itself
			CreatedUtc = item.CreatedUtc,
			UpdatedUtc = item.UpdatedUtc
		};
	}
}