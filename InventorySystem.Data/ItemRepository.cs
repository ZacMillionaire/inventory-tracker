using System.Text.Json.Serialization;
using InventorySystem.Data.Entities;
using InventorySystem.Data.Models;

namespace InventorySystem.Data;

public class ItemRepository
{
	private readonly DatabaseContext _database;
	private readonly TimeProvider _timeProvider;

	public ItemRepository(TimeProvider timeProvider, DatabaseContext dataStorge)
	{
		_timeProvider = timeProvider;
		_database = dataStorge;
	}

	public ItemDto Create(CreateItemRequestDto newItem)
	{
		// var itemDto = new Item()
		// {
		// 	Name = newItem.Name,
		// 	Description = newItem.Description,
		// 	Attributes = AttributeValueDtoToAttribute(newItem.AttributeValues)
		// };

		var createdItem = _database.Items.CreateItem(newItem.Name, newItem.Description /*, TODO: attributes */);

		return ToDto(createdItem);
	}

	private List<EntityAttribute> AttributeValueDtoToAttribute(List<AttributeValueDto> attributeValueDto)
	{
		return [];
		// return attributeValueDto.Select(y => _attributes.FirstOrDefault(x => x.Id == y.AttributeId))
		// 	.Where(x => x != null)
		// 	.ToList();
	}

	public List<ItemDto> Get()
	{
		return [];
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