namespace InventorySystem.Data.Entities;

internal class Item : BaseEntity
{
	public required string Name { get; set; }
	public string? Description { get; set; }
	public DateTimeOffset CreatedUtc { get; set; }
	public DateTimeOffset? UpdatedUtc { get; set; }
	public List<EntityAttribute> Attributes { get; set; } = [];
}