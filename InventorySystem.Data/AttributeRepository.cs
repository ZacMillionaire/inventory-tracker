using InventorySystem.Data.Entities;
using InventorySystem.Data.Interfaces;
using InventorySystem.Data.Models;
using Marten;

namespace InventorySystem.Data;

public class AttributeRepository : IAttributeRepository
{
	private readonly DatabaseContext _database;
	private readonly IDocumentSession _documentSession;
	private readonly TimeProvider _timeProvider;

	public AttributeRepository(TimeProvider timeProvider, DatabaseContext dataStorge, IDocumentSession documentSession)
	{
		_timeProvider = timeProvider;
		_database = dataStorge;
		_documentSession = documentSession;
	}

	public List<AttributeDto> Get()
	{
		return _database.Attributes.GetAttributes();
	}

	public bool AttributeExistsByName(string attributeName)
	{
		return _database.Attributes.AttributeExistsByName(attributeName);
	}

	public async Task<AttributeDto> Create(CreateAttributeDto attribute)
	{
		attribute.KeyName ??= AttributeNameToKeyName(attribute.Name);
		var newAttribute = new EntityAttribute()
		{
			Name = attribute.Name,
			Type = attribute.Type,
			KeyName = attribute.KeyName,
			Id = Guid.CreateVersion7(_timeProvider.GetUtcNow()),
			CreatedUtc = _timeProvider.GetUtcNow(),
		};

		_documentSession.Store(newAttribute);

		await _documentSession.SaveChangesAsync();

		return ToDto(newAttribute);
	}

	private AttributeDto ToDto(EntityAttribute attribute)
	{
		return new AttributeDto()
		{
			Name = attribute.Name,
			Type = attribute.Type,
			KeyName = attribute.KeyName,
			Id = attribute.Id,
			CreatedUtc = attribute.CreatedUtc,
			UpdatedUtc = attribute.UpdatedUtc
		};
	}

	private static string AttributeNameToKeyName(string attributeName)
	{
		return attributeName.Trim().Replace(" ", "_").ToLower();
	}
}