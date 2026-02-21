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

	public async Task<List<AttributeDto>> Get()
	{
		var attributes = await _documentSession.Query<EntityAttribute>()
			.ToListAsync();

		return attributes.Select(ToDto).ToList();
	}

	public async Task<bool> AttributeExistsByName(string attributeName)
	{
		return await _documentSession.Query<EntityAttribute>()
			.AnyAsync(x => x.Name == attributeName);
	}

	public async Task<AttributeDto> Create(CreateAttributeDto attribute)
	{
		attribute.KeyName ??= AttributeNameToKeyName(attribute.Name);

		var newAttribute = await CreateAsyncImpl(new EntityAttribute()
		{
			Name = attribute.Name,
			Type = attribute.Type,
			KeyName = attribute.KeyName,
			Id = Guid.CreateVersion7(_timeProvider.GetUtcNow()),
			CreatedUtc = _timeProvider.GetUtcNow(),
		});

		return ToDto(newAttribute);
	}

	internal async Task<EntityAttribute> CreateAsyncImpl(EntityAttribute attribute)
	{
		_documentSession.Store(attribute);

		await _documentSession.SaveChangesAsync();

		return attribute;
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