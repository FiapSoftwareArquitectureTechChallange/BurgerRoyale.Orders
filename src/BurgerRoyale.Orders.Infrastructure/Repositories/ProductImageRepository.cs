using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Interface.Repositories;
using BurgerRoyale.Orders.Infrastructure.Context;
using BurgerRoyale.Orders.Infrastructure.RepositoriesStandard;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ProductImageRepository(ApplicationDbContext applicationDbContext) : DomainRepository<ProductImage>(applicationDbContext), IProductImageRepository
    {
    }
}