namespace InventorySystem.Data.Entities;

public class Item : BaseEntity
{
	public required string Name { get; set; }
	public string? Description { get; set; }

	/// <summary>
	/// If true, only one instance of this item can exist by name
	/// </summary>
	public bool Distinct { get; set; }

	public string NormalisedName { get; set; } = null!;
	// public List<EntityAttribute> Attributes { get; set; } = [];
}