using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Interface.Repositories;
using BurgerRoyale.Orders.Infrastructure.Context;
using BurgerRoyale.Orders.Infrastructure.RepositoriesStandard;

namespace BurgerRoyale.Orders.Infrastructure.Repositories
{
    public class ProductImageRepository : DomainRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}