using BurgerRoyale.Orders.API.ConfigController;
using BurgerRoyale.Orders.Domain.Constants;
using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Interface.Services;
using BurgerRoyale.Orders.Domain.ResponseDefault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BurgerRoyale.Orders.API.Controllers.Product;

[Authorize(Roles = $"{RolesConstants.Admin},{RolesConstants.Employee}")]
[Route("api/[controller]")]
[ApiController]
public class ProductController : BaseController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [AllowAnonymous]
    [HttpGet]
    [SwaggerOperation(Summary = "Get a list of products", Description = "Retrieves a list of products based on the specified category.")]
    [ProducesResponseType(typeof(IEnumerable<ReturnAPI<ProductDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReturnAPI), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetList([FromQuery] ProductCategory? productCategory)
    {
        IEnumerable<ProductDTO> response = await _productService.GetListAsync(productCategory);

        return IStatusCode(new ReturnAPI<IEnumerable<ProductDTO>>(response));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new product", Description = "Creates a new product.")]
    [ProducesResponseType(typeof(ReturnAPI<ProductDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ReturnAPI), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Add([FromBody] RequestProductDTO productDTO)
    {
        ProductDTO response = await _productService.AddAsync(productDTO);

        return IStatusCode(new ReturnAPI<ProductDTO>(HttpStatusCode.Created, response));
    }

    [HttpGet("{id:Guid}")]
    [SwaggerOperation(Summary = "Get a product by ID", Description = "Retrieves a product by its ID.")]
    [ProducesResponseType(typeof(ReturnAPI<ProductDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReturnAPI), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        ProductDTO response = await _productService.GetByIdAsync(id);

        return IStatusCode(new ReturnAPI<ProductDTO>(response));
    }

    [HttpPut("{id:Guid}")]
    [SwaggerOperation(Summary = "Update a product", Description = "Updates an existing product by its ID.")]
    [ProducesResponseType(typeof(ReturnAPI<ProductDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReturnAPI), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestProductDTO productDTO)
    {
        ProductDTO response = await _productService.UpdateAsync(id, productDTO);

        return IStatusCode(new ReturnAPI<ProductDTO>(response));
    }

    [HttpDelete("{id:Guid}")]
    [SwaggerOperation(Summary = "Delete a product by ID", Description = "Deletes a product by its ID.")]
    [ProducesResponseType(typeof(ReturnAPI<HttpStatusCode>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ReturnAPI), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Remove([FromRoute] Guid id)
    {
        await _productService.RemoveAsync(id);

        return IStatusCode(new ReturnAPI(HttpStatusCode.NoContent));
    }
}