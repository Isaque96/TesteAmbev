using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features;

public class ProductsControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private ProductsController CreateController() => new(_mediator, _mapper);

    #region CreateProduct

    [Fact(DisplayName = "Given invalid create product request, when CreateAsync called, then returns BadRequest")]
    public async Task Given_InvalidCreateProductRequest_When_CreateAsync_Then_BadRequest()
    {
        var controller = CreateController();

        var request = new CreateProductRequest
        {
            Title = string.Empty,
            Price = 0m
        };
        var command = new CreateProductCommand();
        var prodResult = new CreateProductResult();

        _mapper.Map<CreateProductCommand>(request).Returns(command);
        _mediator.Send(command).Returns(prodResult);
        _mapper.Map<CreateProductResponse>(prodResult).Returns(new CreateProductResponse());
        var result = await controller.CreateAsync(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid create product request, when CreateAsync called, then returns Created")]
    public async Task Given_ValidCreateProductRequest_When_CreateAsync_Then_Created()
    {
        var controller = CreateController();

        var request = new CreateProductRequest
        {
            Title = "Produto",
            Price = 10.5m,
            Description = "Descrição do produto",
            CategoryId = Guid.NewGuid(),
            Image = "image.png",
            Rating = new RatingDto
            {
                Rate = 4.2m,
                Count = 150
            }
        };

        var appResult = new CreateProductResult { Id = Guid.NewGuid() };
        var apiResponse = new CreateProductResponse { Id = appResult.Id, Title = request.Title, Price = request.Price };

        _mapper.Map<CreateProductCommand>(request).Returns(new CreateProductCommand());
        _mediator.Send(Arg.Any<CreateProductCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<CreateProductResponse>(appResult).Returns(apiResponse);

        var result = await controller.CreateAsync(request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        Assert.Equal(nameof(ProductsController.GetByIdAsync), created.ActionName);
        Assert.NotNull(created.Value);

        _mapper.Received(1).Map<CreateProductCommand>(request);
        await _mediator.Received(1).Send(Arg.Any<CreateProductCommand>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateProductResponse>(appResult);
    }

    #endregion

    #region GetProduct

    [Fact(DisplayName = "Given invalid id, when GetByIdAsync called, then returns BadRequest")]
    public async Task Given_InvalidId_When_GetByIdAsync_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.GetByIdAsync(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when GetByIdAsync called, then returns Ok with data")]
    public async Task Given_ValidId_When_GetByIdAsync_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var appResult = new GetProductResult { Id = id };
        var apiResponse = new GetProductResponse { Id = id };

        // Controller builds the query itself; we just stub mediator/send result
        _mediator.Send(Arg.Any<GetProductQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<GetProductResponse>(appResult).Returns(apiResponse);

        var result = await controller.GetByIdAsync(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        await _mediator.Received(1).Send(Arg.Any<GetProductQuery>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetProductResponse>(appResult);
    }

    #endregion

    #region ListProducts

    [Fact(DisplayName = "Given list request, when ListAsync called, then returns Ok with list")]
    public async Task Given_ListRequest_When_ListAsync_Then_Ok()
    {
        var controller = CreateController();

        var page = 1;
        var size = 10;
        var appResult = new ListProductsResult();
        var response = new ListProductsResponse();

        _mediator.Send(Arg.Any<ListProductsQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<ListProductsResponse>(appResult).Returns(response);

        var result = await controller.ListAsync(page, size, null, null, null, null, null, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        await _mediator.Received(1).Send(Arg.Any<ListProductsQuery>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<ListProductsResponse>(appResult);
    }

    #endregion

    #region UpdateProduct

    [Fact(DisplayName = "Given id mismatch, when UpdateAsync called, then returns BadRequest")]
    public async Task Given_IdMismatch_When_UpdateAsync_Then_BadRequest()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateProductRequest
        {
            Id = Guid.NewGuid(),
            Title = "Updated",
            Price = 20m,
            Description = "Up",
            CategoryId = Guid.NewGuid(),
            Image = "img.png",
            Rating = new RatingDto
            {
                Rate = 4.5m,
                Count = 100
            }
        };

        var result = await controller.UpdateAsync(id, request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid update request, when UpdateAsync called, then returns Ok")]
    public async Task Given_ValidUpdateRequest_When_UpdateAsync_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateProductRequest
        {
            Id = id,
            Title = "Updated Product",
            Price = 25.0m,
            Description = "Updated description",
            CategoryId = Guid.NewGuid(),
            Image = "updated_image.png",
            Rating = new RatingDto
            {
                Rate = 5.0m,
                Count = 200
            }
        };

        var appResult = new UpdateProductResult { Id = id };
        var apiResponse = new UpdateProductResponse { Id = id };

        _mapper.Map<UpdateProductCommand>(request).Returns(new UpdateProductCommand());
        _mediator.Send(Arg.Any<UpdateProductCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<UpdateProductResponse>(appResult).Returns(apiResponse);

        var result = await controller.UpdateAsync(id, request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<UpdateProductCommand>(request);
        await _mediator.Received(1).Send(Arg.Any<UpdateProductCommand>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UpdateProductResponse>(appResult);
    }

    #endregion

    #region DeleteProduct

    [Fact(DisplayName = "Given invalid id, when DeleteAsync called, then returns BadRequest")]
    public async Task Given_InvalidId_When_DeleteAsync_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.DeleteAsync(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when DeleteAsync called, then returns Ok")]
    public async Task Given_ValidId_When_DeleteAsync_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        _mediator.Send(Arg.Any<DeleteProductCommand>(), Arg.Any<CancellationToken>()).Returns(new DeleteProductResult { Deleted = true });
        // Controller constructs DeleteProductCommand inline; no mapper call required here

        var result = await controller.DeleteAsync(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        await _mediator.Received(1).Send(Arg.Any<DeleteProductCommand>(), Arg.Any<CancellationToken>());
    }

    #endregion
}
