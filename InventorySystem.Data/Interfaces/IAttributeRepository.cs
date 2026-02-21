using InventorySystem.Data.Models;

namespace InventorySystem.Data.Interfaces;

public interface IAttributeRepository
{
	List<AttributeDto> Get();
	Task<bool> AttributeExistsByName(string attributeName);
	Task<AttributeDto> Create(CreateAttributeDto attribute);
}