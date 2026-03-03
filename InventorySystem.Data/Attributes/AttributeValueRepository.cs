using Marten;

namespace InventorySystem.Data.Attributes;

public class AttributeValueRepository
{
	private readonly TimeProvider _timeProvider;
	private readonly IDocumentSession _documentSession;

	public AttributeValueRepository(TimeProvider timeProvider, IDocumentSession documentSession)
	{
		_timeProvider = timeProvider;
		_documentSession = documentSession;
	}

	public void CreateStringValue(string value, Guid attributeId, Guid itemId)
	{
	}
}