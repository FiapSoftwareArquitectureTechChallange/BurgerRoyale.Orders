using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Enumerators;

namespace BurgerRoyale.Orders.Domain.Interface.Services
{
	public interface IProductService
	{
		Task<IEnumerable<ProductDTO>> GetListAsync(ProductCategory? category);

		Task<ProductDTO> AddAsync(RequestProductDTO addProductRequestDTO);

		Task<ProductDTO> GetByIdAsync(Guid id);

		Task<ProductDTO> UpdateAsync(Guid id, RequestProductDTO updateProductRequestDTO);

		Task RemoveAsync(Guid id);
	}
}