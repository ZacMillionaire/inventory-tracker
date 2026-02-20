using InventorySystem.Data.Models;

namespace InventorySystem.Data.Interfaces;

public interface IAttributeRepository
{
	List<AttributeDto> Get();
	bool AttributeExistsByName(string attributeName);
	AttributeDto Create(CreateAttributeDto attribute);
}