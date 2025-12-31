using InventorySystem.Data.Entities;
using InventorySystem.Data.Models;

namespace InventorySystem.Data;

public class ItemRepository
{
	private List<Item> _items = [];
	private List<EntityAttribute> _attributes = [];

	public void Create(CreateItemRequestDto newItem)
	{
		var itemDto = new Item()
		{
			Name = newItem.Name,
			Description = newItem.Description,
			Attributes = AttributeValueDtoToAttribute(newItem.AttributeValues)
		};
	}

	private List<EntityAttribute> AttributeValueDtoToAttribute(List<AttributeValueDto> attributeValueDto)
	{
		return attributeValueDto.Select(y => _attributes.FirstOrDefault(x => x.Id == y.AttributeId))
			.Where(x => x != null)
			.ToList();
	}

	public List<ItemDto> Get()
	{
		return _items.Select(x => new ItemDto()
			{
				Name = x.Name,
				Description = x.Description,
				Id = x.Id,
				Attributes = x.Attributes.Select(y => new AttributeDto()
					{
						Name = y.Name,
						KeyName = y.KeyName,
						Id = y.Id,
						// IntValue = y.IntValue,
						// DoubleValue = y.DoubleValue,
						// StringValue = y.StringValue,
						Type = y.Type
					})
					.ToList()
			})
			.ToList();
	}
}