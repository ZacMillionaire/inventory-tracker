using System.ComponentModel.DataAnnotations.Schema;
using InventorySystem.Data.Enums;

namespace InventorySystem.Data.Entities;

[Table("Attributes")]
internal class EntityAttribute : BaseEntity
{
	public string Name { get; set; } = null!;
	public string KeyName { get; set; } = null!;
	public AttributeType Type { get; set; }
}

internal class EntityAttributeValue : BaseEntity
{
	public int AttributeId { get; set; }
	public string StringValue { get; set; }
	public int IntValue { get; set; }
	public double DoubleValue { get; set; }
}