using Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;
using Ambev.DeveloperEvaluation.Application.Categories.DeleteCategory;
using Ambev.DeveloperEvaluation.Application.Categories.GetCategory;
using Ambev.DeveloperEvaluation.Application.Categories.ListCategories;
using Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features;

public class CategoriesControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private CategoriesController CreateController() => new(_mediator, _mapper);

    #region CreateCategory

    [Fact(DisplayName = "Given invalid create category request, when CreateCategory called, then returns BadRequest")]
    public async Task Given_InvalidCreateCategoryRequest_When_CreateCategory_Then_BadRequest()
    {
        var controller = CreateController();

        var request = new CreateCategoryRequest(); // não inicializa campos para não assumir propriedades
        // validator dentro da controller irá validar; adiciona ModelState só por precaução
        controller.ModelState.AddModelError("Name", "Required");

        var result = await controller.CreateCategory(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid create category request, when CreateCategory called, then returns Created")]
    public async Task Given_ValidCreateCategoryRequest_When_CreateCategory_Then_Created()
    {
        var controller = CreateController();

        var request = new CreateCategoryRequest
        {
            Name = "Bebidas",
            Description = "Categoria de bebidas"
        };

        var appResult = new CreateCategoryResult { Id = Guid.NewGuid() };
        var apiResponse = new CreateCategoryResponse { Id = appResult.Id };

        _mapper.Map<CreateCategoryCommand>(request).Returns(new CreateCategoryCommand());
        _mediator.Send(Arg.Any<CreateCategoryCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<CreateCategoryResponse>(appResult).Returns(apiResponse);

        var result = await controller.CreateCategory(request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        Assert.Equal(nameof(CategoriesController.GetCategory), created.ActionName);
        Assert.NotNull(created.Value);

        _mapper.Received(1).Map<CreateCategoryCommand>(request);
        await _mediator.Received(1).Send(Arg.Any<CreateCategoryCommand>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateCategoryResponse>(appResult);
    }

    #endregion

    #region GetCategory

    [Fact(DisplayName = "Given invalid id, when GetCategory called, then returns BadRequest")]
    public async Task Given_InvalidId_When_GetCategory_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.GetCategory(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when GetCategory called, then returns Ok with data")]
    public async Task Given_ValidId_When_GetCategory_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var appResult = new GetCategoryResult { Id = id };
        var apiResponse = new GetCategoryResponse { Id = id };

        _mapper.Map<GetCategoryQuery>(id).Returns(new GetCategoryQuery());
        _mediator.Send(Arg.Any<GetCategoryQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<GetCategoryResponse>(appResult).Returns(apiResponse);

        var result = await controller.GetCategory(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<GetCategoryQuery>(id);
        await _mediator.Received(1).Send(Arg.Any<GetCategoryQuery>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetCategoryResponse>(appResult);
    }

    #endregion

    #region ListCategories

    [Fact(DisplayName = "Given list request, when ListCategories called, then returns Ok with list")]
    public async Task Given_ListRequest_When_ListCategories_Then_Ok()
    {
        var controller = CreateController();

        var request = new ListCategoriesRequest();
        var appResult = new List<string>();
        var apiResponse = new ListCategoriesResponse();

        _mapper.Map<ListCategoriesQuery>(request).Returns(new ListCategoriesQuery());
        _mediator.Send(Arg.Any<ListCategoriesQuery>(), Arg.Any<CancellationToken>()).Returns(appResult);
        _mapper.Map<ListCategoriesResponse>(appResult).Returns(apiResponse);

        var result = await controller.ListCategories(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<ListCategoriesQuery>(request);
        await _mediator.Received(1).Send(Arg.Any<ListCategoriesQuery>(), Arg.Any<CancellationToken>());
    }

    #endregion

    #region UpdateCategory

    [Fact(DisplayName = "Given id mismatch, when UpdateCategory called, then returns BadRequest")]
    public async Task Given_IdMismatch_When_UpdateCategory_Then_BadRequest()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateCategoryRequest { Id = Guid.NewGuid() };

        var result = await controller.UpdateCategory(id, request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid update request, when UpdateCategory called, then returns Ok")]
    public async Task Given_ValidUpdateRequest_When_UpdateCategory_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateCategoryRequest { Id = id, Name = "Updated Name", Description = "Updated Description" };

        var appResult = new UpdateCategoryResult { Id = id };
        var apiResponse = new UpdateCategoryResponse { Id = id };

        _mapper.Map<UpdateCategoryCommand>(request).Returns(new UpdateCategoryCommand());
        _mediator.Send(Arg.Any<UpdateCategoryCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<UpdateCategoryResponse>(appResult).Returns(apiResponse);

        var result = await controller.UpdateCategory(id, request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<UpdateCategoryCommand>(request);
        await _mediator.Received(1).Send(Arg.Any<UpdateCategoryCommand>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UpdateCategoryResponse>(appResult);
    }

    #endregion

    #region DeleteCategory

    [Fact(DisplayName = "Given invalid id, when DeleteCategory called, then returns BadRequest")]
    public async Task Given_InvalidId_When_DeleteCategory_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.DeleteCategory(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when DeleteCategory called, then returns Ok")]
    public async Task Given_ValidId_When_DeleteCategory_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        _mapper.Map<DeleteCategoryCommand>(id).Returns(new DeleteCategoryCommand());
        _mediator.Send(Arg.Any<DeleteCategoryCommand>(), Arg.Any<CancellationToken>()).Returns(new DeleteCategoryResult { Deleted = true });

        var result = await controller.DeleteCategory(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<DeleteCategoryCommand>(id);
        await _mediator.Received(1).Send(Arg.Any<DeleteCategoryCommand>(), Arg.Any<CancellationToken>());
    }

    #endregion
}
