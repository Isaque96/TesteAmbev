using Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;
using Ambev.DeveloperEvaluation.Application.Categories.GetCategory;
using Ambev.DeveloperEvaluation.Application.Categories.ListCategories;
using Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;
using Ambev.DeveloperEvaluation.Application.Categories.DeleteCategory;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.DeleteCategory;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IMediator mediator, IMapper mapper) : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCategoryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<CreateCategoryCommand>(request);
        var response = await mediator.Send(command, cancellationToken);

        return Created(
            nameof(GetCategory),
            response,
            mapper.Map<CreateCategoryResponse>(response),
            "Category created successfully"
        );
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCategoryRequest { Id = id };
        var validator = new GetCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<GetCategoryQuery>(request.Id);
        var response = await mediator.Send(command, cancellationToken);

        return Ok(mapper.Map<GetCategoryResponse>(response), "Category retrieved successfully");
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListCategoriesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCategories([FromQuery] ListCategoriesRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<ListCategoriesQuery>(request);
        var response = await mediator.Send(command, cancellationToken);

        return Ok(mapper.Map<ListCategoriesResponse>(response), "Categories listed successfully");
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest("Id in URL and payload do not match.");

        var validator = new UpdateCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<UpdateCategoryCommand>(request);
        var response = await mediator.Send(command, cancellationToken);

        return Ok(mapper.Map<UpdateCategoryResponse>(response), "Category updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteCategoryRequest { Id = id };
        var validator = new DeleteCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<DeleteCategoryCommand>(request.Id);
        await mediator.Send(command, cancellationToken);

        return Ok<object>(message: "Category deleted successfully");
    }
}