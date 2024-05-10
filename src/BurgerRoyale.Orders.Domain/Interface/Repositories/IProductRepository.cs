using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Interface.RepositoriesStandard;

namespace BurgerRoyale.Orders.Domain.Interface.Repositories
{
    public interface IProductRepository : IDomainRepository<Product>
	{
		Task <IEnumerable<Product>> GetAll();
		Task <IEnumerable<Product>> GetAllByCategory(ProductCategory category);
		Task <Product?> GetProductById(Guid id);
	}
}