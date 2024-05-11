using Bogus;
using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;

namespace BurgerRoyale.Orders.UnitTests.Domain.EntitiesMocks
{
    public static class ProductMock
    {
        public static Product Get
        (
            Guid? id = null,
            string? name = null,
            string? description = null,
            decimal? price = null,
            ProductCategory? category = null
        )
        {
            return new Faker<Product>()
                .CustomInstantiator(faker => new Product(
                    id ?? Guid.NewGuid(),
                    name ?? faker.Commerce.ProductName(),
                    description ?? faker.Commerce.ProductDescription(),
                    price ?? decimal.Parse(faker.Commerce.Price()),
                    category ?? faker.PickRandom<ProductCategory>()
                ));
        }
    }
}
