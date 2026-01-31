namespace InventorySystem.Data.Entities;

internal abstract class BaseEntity
{
	public Guid Id { get; set; }
	public DateTimeOffset CreatedUtc { get; set; }
	public DateTimeOffset? UpdatedUtc { get; set; }
}