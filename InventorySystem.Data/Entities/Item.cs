namespace InventorySystem.Data.Entities;

public class Item : BaseEntity
{
	public required string Name { get; set; }
	public string? Description { get; set; }
	// public List<EntityAttribute> Attributes { get; set; } = [];
}