using System.Security.Claims;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Common;

public class TestBaseController : BaseController
{
    // Apenas expõe os métodos protegidos como públicos para podermos testar
    public int PublicGetCurrentUserId() => GetCurrentUserId();
    public string PublicGetCurrentUserEmail() => GetCurrentUserEmail();

    public IActionResult PublicOk<T>(T? data = default, string message = "Sucesso", bool success = true)
        => Ok(data, message, success);

    public IActionResult PublicCreated<T>(string actionName, object routeValues, T? data = default, string message = "Sucesso")
        => Created(actionName, routeValues, data, message);

    public IActionResult PublicBadRequest(string message)
        => BadRequest(message);

    public IActionResult PublicNotFound(string message = "Resource not found")
        => NotFound(message);

    public IActionResult PublicOkPaginated<T>(PaginatedList<T> pagedList)
        => OkPaginated(pagedList);
}

public class BaseControllerTests
{
    private static TestBaseController CreateControllerWithUser(params Claim[] claims)
    {
        var controller = new TestBaseController();

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"))
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    [Fact(DisplayName = "Given user with NameIdentifier claim, when GetCurrentUserId called, then returns parsed int value")]
    public void Given_UserWithNameIdentifierClaim_When_GetCurrentUserId_Then_ReturnsInt()
    {
        // Arrange
        var controller = CreateControllerWithUser(
            new Claim(ClaimTypes.NameIdentifier, "123")
        );

        // Act
        var userId = controller.PublicGetCurrentUserId();

        // Assert
        Assert.Equal(123, userId);
    }

    [Fact(DisplayName = "Given user with Email claim, when GetCurrentUserEmail called, then returns email value")]
    public void Given_UserWithEmailClaim_When_GetCurrentUserEmail_Then_ReturnsEmail()
    {
        // Arrange
        var controller = CreateControllerWithUser(
            new Claim(ClaimTypes.Email, "user@test.com")
        );

        // Act
        var email = controller.PublicGetCurrentUserEmail();

        // Assert
        Assert.Equal("user@test.com", email);
    }

    [Fact(DisplayName = "Given Ok called with default parameters, when executed, then returns ApiResponseWithData with default message and success true")]
    public void Given_OkCalledWithDefaults_When_Executed_Then_ReturnsApiResponseWithData()
    {
        // Arrange
        var controller = new TestBaseController();

        // Act
        var result = controller.PublicOk<string>();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponseWithData<string>>(okResult.Value);

        Assert.True(response.Success);
        Assert.Equal("Sucesso", response.Message);
        Assert.Null(response.Data);
    }

    [Fact(DisplayName = "Given Ok called with data, when executed, then wraps data in ApiResponseWithData")]
    public void Given_OkCalledWithData_When_Executed_Then_WrapsData()
    {
        // Arrange
        var controller = new TestBaseController();
        const string data = "result";

        // Act
        var result = controller.PublicOk(data, "Mensagem ok");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponseWithData<string>>(okResult.Value);

        Assert.True(response.Success);
        Assert.Equal("Mensagem ok", response.Message);
        Assert.Equal(data, response.Data);
    }

    [Fact(DisplayName = "Given Created called, when executed, then returns CreatedAtAction with ApiResponseWithData")]
    public void Given_CreatedCalled_When_Executed_Then_ReturnsCreatedAtActionWithApiResponseWithData()
    {
        // Arrange
        var controller = new TestBaseController();
        var data = new User { Id = Guid.Empty, Username = "Item" };
        object routeValues = new { id = 1 };

        // Act
        var result = controller.PublicCreated("GetById", routeValues, data, "Created successfully");

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdResult.ActionName);

        Assert.NotNull(createdResult.RouteValues);
        Assert.True(createdResult.RouteValues.ContainsKey("id"));
        Assert.Equal(1, createdResult.RouteValues["id"]);

        var response = Assert.IsType<ApiResponseWithData<User>>(createdResult.Value);

        Assert.True(response.Success);
        Assert.Equal("Created successfully", response.Message);
        Assert.NotNull(response.Data);
        Assert.Equal(data.Id, response.Data.Id);
        Assert.Equal(data.Name, response.Data.Name);
    }

    [Fact(DisplayName = "Given BadRequest called, when executed, then returns BadRequestObjectResult with ApiResponse")]
    public void Given_BadRequestCalled_When_Executed_Then_ReturnsBadRequestWithApiResponse()
    {
        // Arrange
        var controller = new TestBaseController();
        const string message = "Invalid data";

        // Act
        var result = controller.PublicBadRequest(message);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ApiResponse>(badRequestResult.Value);

        Assert.False(response.Success);
        Assert.Equal(message, response.Message);
    }

    [Fact(DisplayName = "Given NotFound called with default message, when executed, then returns NotFoundObjectResult with ApiResponse")]
    public void Given_NotFoundCalledWithDefaultMessage_When_Executed_Then_ReturnsNotFoundWithApiResponse()
    {
        // Arrange
        var controller = new TestBaseController();

        // Act
        var result = controller.PublicNotFound();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ApiResponse>(notFoundResult.Value);

        Assert.False(response.Success);
        Assert.Equal("Resource not found", response.Message);
    }

    [Fact(DisplayName = "Given NotFound called with custom message, when executed, then returns NotFoundObjectResult with ApiResponse")]
    public void Given_NotFoundCalledWithCustomMessage_When_Executed_Then_ReturnsNotFoundWithCustomMessage()
    {
        // Arrange
        var controller = new TestBaseController();
        const string message = "User not found";

        // Act
        var result = controller.PublicNotFound(message);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ApiResponse>(notFoundResult.Value);

        Assert.False(response.Success);
        Assert.Equal(message, response.Message);
    }

    [Fact(DisplayName = "Given PaginatedList, when OkPaginated called, then returns PaginatedResponse with correct metadata")]
    public void Given_PaginatedList_When_OkPaginatedCalled_Then_ReturnsPaginatedResponse()
    {
        // Arrange
        var controller = new TestBaseController();
        var items = new List<string> { "Item1", "Item2" };

        // Ajuste o construtor conforme sua implementação de PaginatedList<T>
        var pagedList = new PaginatedList<string>(items, count: 10, pageNumber: 2, pageSize: 2);

        // Act
        var result = controller.PublicOkPaginated(pagedList);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponseWithData<PaginatedResponse<string>>>(okResult.Value);

        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Same(pagedList, response.Data.Data);
        Assert.NotNull(response.Data);
        Assert.Equal(pagedList.CurrentPage, response.Data.CurrentPage);
        Assert.Equal(pagedList.TotalPages, response.Data.TotalPages);
        Assert.Equal(pagedList.TotalCount, response.Data.TotalCount);
    }
}