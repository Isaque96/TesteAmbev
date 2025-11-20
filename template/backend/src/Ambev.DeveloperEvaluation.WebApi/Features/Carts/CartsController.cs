using Ambev.DeveloperEvaluation.Application.Cart.CreateCart;
using Ambev.DeveloperEvaluation.Application.Cart.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Cart.GetCart;
using Ambev.DeveloperEvaluation.Application.Cart.ListCarts;
using Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

[ApiController]
[Route("api/[controller]")]
public class CartsController(IMediator mediator, IMapper mapper) : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<CreateCartCommand>(request);
        var response = await mediator.Send(command, cancellationToken);

        return Created(
            nameof(GetCart),
            response,
            mapper.Map<CreateCartResponse>(response),
            "Cart created successfully"
        );
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCartRequest { Id = id };
        var validator = new GetCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<GetCartQuery>(request.Id);
        var response = await mediator.Send(command, cancellationToken);

        return Ok(mapper.Map<GetCartResponse>(response), "Cart retrieved successfully");
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListCartsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCarts([FromQuery] ListCartsRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<ListCartsQuery>(request);
        var response = await mediator.Send(command, cancellationToken);

        return Ok(mapper.Map<ListCartsResponse>(response), "Carts listed successfully");
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCart([FromRoute] Guid id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest("Id in URL and payload do not match.");

        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<UpdateCartCommand>(request);
        var response = await mediator.Send(command, cancellationToken);

        return Ok(mapper.Map<UpdateCartResponse>(response), "Cart updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteCartRequest { Id = id };
        var validator = new DeleteCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = mapper.Map<DeleteCartCommand>(request.Id);
        await mediator.Send(command, cancellationToken);

        return Ok<object>(message: "Cart deleted successfully");
    }
}