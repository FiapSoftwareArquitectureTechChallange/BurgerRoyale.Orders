﻿using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Helpers;

namespace BurgerRoyale.Orders.Domain.DTO;

public class ProductDTO
{
    public ProductDTO() { }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ProductCategory Category { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public IEnumerable<ProductImageDTO> Images { get; set; } = Enumerable.Empty<ProductImageDTO>();

    public string CategoryDescription
    {
        get => Category.GetDescription();
    }

    public ProductDTO(Product product)
    {
        if (product.Images != null)
        {
            var images = product.Images.Select(x => new ProductImageDTO(x.Title, x.Url));
            Images = images;
        }
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Price = product.Price;
        Category = product.Category;
    }
}