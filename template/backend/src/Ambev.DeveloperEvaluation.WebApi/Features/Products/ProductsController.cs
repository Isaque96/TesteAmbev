using Ambev.DeveloperEvaluation.Application.Categories.ListCategories;
using Ambev.DeveloperEvaluation.Application.Categories.ListProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

/// <summary>
/// Controller for managing products
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator, IMapper mapper) : BaseController
{
    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="request">Product creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created product details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProduct.CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateProduct.CreateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var validator = new CreateProduct.CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var command = mapper.Map<CreateProductCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        var response = mapper.Map<CreateProduct.CreateProductResponse>(result);

        return Created(
            nameof(GetByIdAsync),
            new {response.Id},
            response,
            "Product created successfully"
        );
    }

    /// <summary>
    /// Retrieves a product by ID
    /// </summary>
    /// <param name="id">Product unique identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProduct.GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetProductRequest { Id = id };
        var validator = new GetProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = mapper.Map<GetProductQuery>(request);
        var result = await mediator.Send(query, cancellationToken);
        var response = mapper.Map<GetProduct.GetProductResponse>(result);

        return Ok(response, "Product found");
    }

    /// <summary>
    /// Retrieves a paginated list of products with optional filtering and ordering
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="order">Ordering (e.g., "price desc, title asc")</param>
    /// <param name="title">Filter by title (supports * for partial match)</param>
    /// <param name="category">Filter by category</param>
    /// <param name="minPrice">Minimum price filter</param>
    /// <param name="maxPrice">Maximum price filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListProducts.ListProductsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? order = null,
        [FromQuery] string? title = null,
        [FromQuery] string? category = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListProductsQuery
        {
            Page = page,
            Size = size,
            Order = order,
            Title = title,
            Category = category,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };

        var result = await mediator.Send(query, cancellationToken);
        var response = mapper.Map<ListProducts.ListProductsResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">Product unique identifier</param>
    /// <param name="request">Product update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated product details</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        if (id != request.Id)
            return BadRequest("Id in URL and payload do not match.");

        var validator = new UpdateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var command = mapper.Map<UpdateProductCommand>(request);
        command.Id = id;

        var result = await mediator.Send(command, cancellationToken);
        var response = mapper.Map<UpdateProductResponse>(result);

        return Ok(response, "Product updated successfully");
    }

    /// <summary>
    /// Deletes a product
    /// </summary>
    /// <param name="id">Product unique identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion confirmation</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var request = new DeleteProductRequest { Id = id };
        var validator = new DeleteProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var command = mapper.Map<DeleteProductCommand>(request);
        await mediator.Send(command, cancellationToken);

        return Ok<object>("Product deleted successfully");
    }

    /// <summary>
    /// Retrieves all product categories
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of categories</returns>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<string>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var query = new ListCategoriesQuery();
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves products by category with pagination
    /// </summary>
    /// <param name="category">CategoryName name</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="order">Ordering (e.g., "price desc, title asc")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products in the category</returns>
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(ApiResponseWithData<ListProductsByCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategoryAsync(
        [FromRoute] string category,
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int size = 10,
        [FromQuery(Name = "_order")] string? order = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = new ListProductsByCategoryQuery
        {
            CategoryName = category,
            Page = page,
            Size = size,
            Order = order
        };

        var result = await mediator.Send(query, cancellationToken);
        var response = mapper.Map<ListProductsByCategoryResponse>(result);

        return Ok(response);
    }
}