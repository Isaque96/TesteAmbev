using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features;

public class AuthControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private AuthController CreateController()
    {
        return new AuthController(_mediator, _mapper);
    }

    [Fact(DisplayName = "Given invalid request, when AuthenticateUser called, then returns BadRequest with validation errors")]
    public async Task Given_InvalidRequest_When_AuthenticateUser_Then_ReturnsBadRequest()
    {
        // Arrange
        var controller = CreateController();

        var request = new AuthenticateUserRequest
        {
            Email = "",          // inválido
            Password = ""        // inválido
        };

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await controller.AuthenticateUser(request, cancellationToken);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid request, when AuthenticateUser called, then returns Ok with response and message")]
    public async Task Given_ValidRequest_When_AuthenticateUser_Then_ReturnsOkWithResponse()
    {
        // Arrange
        var controller = CreateController();

        var request = new AuthenticateUserRequest
        {
            Email = "user@test.com",
            Password = "123456"
        };

        var command = new AuthenticateUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var domainUser = new AuthenticateUserResult
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = "Test User",
            Role = nameof(UserRole.Admin)
        };

        var mappedResponse = new AuthenticateUserResponse
        {
            Token = "jwt-token-123",
            Email = domainUser.Email,
            Name = domainUser.Name,
            Role = domainUser.Role
        };

        _mapper.Map<AuthenticateUserCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
                 .Returns(domainUser);
        _mapper.Map<AuthenticateUserResponse>(domainUser)
               .Returns(mappedResponse);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await controller.AuthenticateUser(request, cancellationToken);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var apiResponse = Assert.IsType<ApiResponseWithData<AuthenticateUserResponse>>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal("User authenticated successfully", apiResponse.Message);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal("jwt-token-123", apiResponse.Data!.Token);
        Assert.Equal(request.Email, apiResponse.Data.Email);
        Assert.Equal("Test User", apiResponse.Data.Name);
        Assert.Equal(domainUser.Role, apiResponse.Data.Role);

        await _mediator.Received(1).Send(command, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<AuthenticateUserCommand>(request);
        _mapper.Received(1).Map<AuthenticateUserResponse>(domainUser);
    }
}