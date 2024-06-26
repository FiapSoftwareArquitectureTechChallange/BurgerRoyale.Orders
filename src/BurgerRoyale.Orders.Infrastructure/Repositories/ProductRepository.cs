﻿using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Interface.Repositories;
using BurgerRoyale.Orders.Infrastructure.Context;
using BurgerRoyale.Orders.Infrastructure.RepositoriesStandard;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ProductRepository(ApplicationDbContext applicationDbContext) : DomainRepository<Product>(applicationDbContext), IProductRepository
    {
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.Include(x => x.Images).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCategory(ProductCategory category)
        {
            return await _context.Products.Include(x => x.Images).Where(x => x.Category == category).ToListAsync();
        }

        public async Task<Product?> GetProductById(Guid id)
        {
            return await _context.Products.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
