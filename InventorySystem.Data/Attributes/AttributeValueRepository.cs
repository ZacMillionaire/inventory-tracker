namespace InventorySystem.Data.Attributes;

public class AttributeValueRepository
{
	private readonly DatabaseContext _database;
	private readonly TimeProvider _timeProvider;

	public AttributeValueRepository(TimeProvider timeProvider, DatabaseContext dataStorge)
	{
		_timeProvider = timeProvider;
		_database = dataStorge;
	}

	public void CreateStringValue(string value, Guid attributeId, Guid itemId)
	{
	}
}