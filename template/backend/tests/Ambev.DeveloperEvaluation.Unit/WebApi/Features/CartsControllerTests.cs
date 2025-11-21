using Ambev.DeveloperEvaluation.Application.Cart.CreateCart;
using Ambev.DeveloperEvaluation.Application.Cart.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Cart.GetCart;
using Ambev.DeveloperEvaluation.Application.Cart.ListCarts;
using Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features;

public class CartsControllerTests
{
    private static readonly IMediator Mediator = Substitute.For<IMediator>();
    private static readonly IMapper Mapper = Substitute.For<IMapper>();

    private static CartsController CreateController()
        => new(Mediator, Mapper);

    #region CreateCart

    [Fact(DisplayName = "Given invalid create cart request, when CreateCart called, then returns BadRequest")]
    public async Task Given_InvalidCreateCartRequest_When_CreateCart_Then_BadRequest()
    {
        var controller = CreateController();

        var request = new CreateCartRequest
        {
            UserId = Guid.Empty,
            Date = DateTime.MinValue,
            Products = []
        };

        var result = await controller.CreateCart(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid create cart request, when CreateCart called, then returns Created")]
    public async Task Given_ValidCreateCartRequest_When_CreateCart_Then_Created()
    {
        var controller = CreateController();

        var request = new CreateCartRequest
        {
            UserId = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            Products = [new CartItemDto { ProductId = Guid.NewGuid(), Quantity = 1 }]
        };

        var appResult = new CreateCartResult();
        var apiResponse = new CreateCartResponse();

        Mapper.Map<CreateCartCommand>(request).Returns(new CreateCartCommand());
        Mediator.Send(Arg.Any<CreateCartCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        Mapper.Map<CreateCartResponse>(appResult).Returns(apiResponse);

        var result = await controller.CreateCart(request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        Assert.Equal(nameof(CartsController.GetCart), created.ActionName);
        Assert.NotNull(created.Value);

        Mapper.Received(1).Map<CreateCartCommand>(request);
        await Mediator.Received(1).Send(Arg.Any<CreateCartCommand>(), Arg.Any<CancellationToken>());
        Mapper.Received(1).Map<CreateCartResponse>(appResult);
    }

    #endregion

    #region GetCart

    [Fact(DisplayName = "Given invalid id, when GetCart called, then returns BadRequest")]
    public async Task Given_InvalidId_When_GetCart_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.GetCart(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when GetCart called, then returns Ok with data")]
    public async Task Given_ValidId_When_GetCart_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var appResult = new GetCartResult();
        var apiResponse = new GetCartResponse();

        Mapper.Map<GetCartQuery>(id).Returns(new GetCartQuery());
        Mediator.Send(Arg.Any<GetCartQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        Mapper.Map<GetCartResponse>(appResult).Returns(apiResponse);

        var result = await controller.GetCart(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        Mapper.Received(1).Map<GetCartQuery>(id);
        await Mediator.Received(1).Send(Arg.Any<GetCartQuery>(), Arg.Any<CancellationToken>());
        Mapper.Received(1).Map<GetCartResponse>(appResult);
    }

    #endregion

    #region ListCarts

    [Fact(DisplayName = "Given list request, when ListCarts called, then returns Ok with list")]
    public async Task Given_ListRequest_When_ListCarts_Then_Ok()
    {
        var controller = CreateController();

        var request = new ListCartsRequest { Page = 1, Size = 10 };
        var appResult = new ListCartsResult();
        var responseList = new List<ListCartsResponse>();

        Mapper.Map<ListCartsQuery>(request).Returns(new ListCartsQuery());
        Mediator.Send(Arg.Any<ListCartsQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        Mapper.Map<List<ListCartsResponse>>(appResult.Data).Returns(responseList);

        var result = await controller.ListCarts(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        Mapper.Received(1).Map<ListCartsQuery>(request);
        await Mediator.Received(1).Send(Arg.Any<ListCartsQuery>(), Arg.Any<CancellationToken>());
    }

    #endregion

    #region UpdateCart

    [Fact(DisplayName = "Given id mismatch, when UpdateCart called, then returns BadRequest")]
    public async Task Given_IdMismatch_When_UpdateCart_Then_BadRequest()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateCartRequest { Id = Guid.NewGuid() };

        var result = await controller.UpdateCart(id, request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid update request, when UpdateCart called, then returns Ok")]
    public async Task Given_ValidUpdateRequest_When_UpdateCart_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateCartRequest { Id = id, UserId = Guid.NewGuid(), Date = DateTime.Now, Products = [ new CartItemDto { ProductId = Guid.NewGuid(), Quantity = 10 } ] };

        var appResult = new UpdateCartResult();
        var apiResponse = new UpdateCartResponse();

        Mapper.Map<UpdateCartCommand>(request).Returns(new UpdateCartCommand());
        Mediator.Send(Arg.Any<UpdateCartCommand>(), Arg.Any<CancellationToken>()).Returns(appResult);
        Mapper.Map<UpdateCartResponse>(appResult).Returns(apiResponse);

        var result = await controller.UpdateCart(id, request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        Mapper.Received(1).Map<UpdateCartCommand>(request);
        await Mediator.Received(1).Send(Arg.Any<UpdateCartCommand>(), Arg.Any<CancellationToken>());
        Mapper.Received(1).Map<UpdateCartResponse>(appResult);
    }

    #endregion

    #region DeleteCart

    [Fact(DisplayName = "Given invalid id, when DeleteCart called, then returns BadRequest")]
    public async Task Given_InvalidId_When_DeleteCart_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.DeleteCart(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when DeleteCart called, then returns Ok")]
    public async Task Given_ValidId_When_DeleteCart_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        Mapper.Map<DeleteCartCommand>(id).Returns(new DeleteCartCommand());
        Mediator.Send(Arg.Any<DeleteCartCommand>(), Arg.Any<CancellationToken>()).Returns(new DeleteCartResult { Deleted = true});

        var result = await controller.DeleteCart(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        Mapper.Received(1).Map<DeleteCartCommand>(id);
        await Mediator.Received(1).Send(Arg.Any<DeleteCartCommand>(), Arg.Any<CancellationToken>());
    }

    #endregion
}