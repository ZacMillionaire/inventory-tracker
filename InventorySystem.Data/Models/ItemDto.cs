using InventorySystem.Data.Enums;

namespace InventorySystem.Data.Models;

public class ItemDto : BaseItemDto
{
	public required string Name { get; set; }
	public string? Description { get; set; }
	public List<AttributeDto> Attributes { get; set; } = [];
}

public class CreateItemRequestDto
{
	public required string Name { get; set; }
	public string? Description { get; set; }
	public List<AttributeValueDto> AttributeValues { get; set; } = [];
}

public class AttributeValueDto
{
	public Guid AttributeId { get; set; }
	public object Value { get; set; }
}

public class CreateAttributeDto
{
	public required string Name { get; set; }

	/// <summary>
	/// If null, <see cref="Name"/> will be convereted to a suitable key name format
	/// </summary>
	public string? KeyName { get; set; }

	public required AttributeType Type { get; set; }
}

public class AttributeDto : BaseItemDto
{
	public required string Name { get; set; }
	public required string KeyName { get; set; }

	public AttributeType Type { get; set; }
	// public string StringValue { get; set; }
	// public int IntValue { get; set; }
	// public double DoubleValue { get; set; }
}

public abstract class BaseItemDto
{
	public Guid Id { get; set; }
}