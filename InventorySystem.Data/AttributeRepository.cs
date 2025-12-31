using InventorySystem.Data.Entities;
using InventorySystem.Data.Models;

namespace InventorySystem.Data;

public class AttributeRepository
{
	private int _idCounter = 1;
	private readonly List<EntityAttribute> _attributes = [];
	private readonly DatabaseContext _database;

	public AttributeRepository(DatabaseContext dataStorge)
	{
		_database = dataStorge;
	}

	public List<AttributeDto> Get()
	{
		return _attributes.Select(ToDto).ToList();
	}

	public bool AttributeExistsByName(string attributeName)
	{
		return _attributes.Any(x => x.Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase));
	}

	public AttributeDto Create(CreateAttributeDto attribute)
	{
		attribute.KeyName ??= AttributeNameToKeyName(attribute.Name);

		var a = new EntityAttribute()
		{
			Name = attribute.Name,
			Type = attribute.Type,
			KeyName = attribute.KeyName,
			Id = _idCounter++,
		};

		_database.CreateEntity(a);

		return ToDto(a);
	}

	private AttributeDto ToDto(EntityAttribute attribute)
	{
		return new AttributeDto()
		{
			Name = attribute.Name,
			Type = attribute.Type,
			KeyName = attribute.KeyName,
			Id = attribute.Id
		};
	}

	private static string AttributeNameToKeyName(string attributeName)
	{
		return attributeName.Trim().Replace(" ", "_").ToLower();
	}
}