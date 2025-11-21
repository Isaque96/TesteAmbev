using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features;

public class UsersControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private UsersController CreateController() => new(_mediator, _mapper);

    #region CreateUser

    [Fact(DisplayName = "Given invalid create user request, when CreateUser called, then returns BadRequest")]
    public async Task Given_InvalidCreateUserRequest_When_CreateUser_Then_BadRequest()
    {
        var controller = CreateController();
        var request = new CreateUserRequest(); // não inicializa campos; validator deve falhar

        var result = await controller.CreateUser(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid create user request, when CreateUser called, then returns Created")]
    public async Task Given_ValidCreateUserRequest_When_CreateUser_Then_Created()
    {
        var controller = CreateController();
        var user = UserTestData.GenerateValidUser();
        var request = new CreateUserRequest
        {
            Email = user.Email,
            Password = user.Password,
            Phone =  user.Phone,
            Role = user.Role,
            Status = user.Status,
            Username = user.Username
        };

        var appResult = new CreateUserResult { Id = Guid.NewGuid() };
        var apiResponse = new CreateUserResponse { Id = appResult.Id };

        _mapper.Map<CreateUserCommand>(request).Returns(new CreateUserCommand());
        _mediator.Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<CreateUserResponse>(appResult).Returns(apiResponse);

        var result = await controller.CreateUser(request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        Assert.Equal(nameof(UsersController.GetUser), created.ActionName);
        Assert.NotNull(created.Value);

        _mapper.Received(1).Map<CreateUserCommand>(request);
        await _mediator.Received(1).Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateUserResponse>(appResult);
    }

    #endregion

    #region GetUser

    [Fact(DisplayName = "Given invalid id, when GetUser called, then returns BadRequest")]
    public async Task Given_InvalidId_When_GetUser_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.GetUser(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when GetUser called, then returns Ok with data")]
    public async Task Given_ValidId_When_GetUser_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var appResult = new GetUserResult { Id = id };
        var apiResponse = new GetUserResponse { Id = id };

        _mapper.Map<GetUserCommand>(Arg.Any<Guid>()).Returns(new GetUserCommand());
        _mediator.Send(Arg.Any<GetUserCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<GetUserResponse>(appResult).Returns(apiResponse);

        var result = await controller.GetUser(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<GetUserCommand>(Arg.Any<Guid>());
        await _mediator.Received(1).Send(Arg.Any<GetUserCommand>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetUserResponse>(appResult);
    }

    #endregion

    #region ListUsers

    [Fact(DisplayName = "Given list request, when ListUsers called, then returns Ok with list")]
    public async Task Given_ListRequest_When_ListUsers_Then_Ok()
    {
        var controller = CreateController();
        var request = new ListUsersRequest(); // usa consulta default

        var appResult = new ListUsersResult();
        var apiResponse = new ListUsersResponse();

        _mapper.Map<ListUsersQuery>(request).Returns(new ListUsersQuery());
        _mediator.Send(Arg.Any<ListUsersQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<ListUsersResponse>(appResult).Returns(apiResponse);

        var result = await controller.ListUsers(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<ListUsersQuery>(request);
        await _mediator.Received(1).Send(Arg.Any<ListUsersQuery>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<ListUsersResponse>(appResult);
    }

    #endregion

    #region UpdateUser

    [Fact(DisplayName = "Given id mismatch, when UpdateUser called, then returns BadRequest")]
    public async Task Given_IdMismatch_When_UpdateUser_Then_BadRequest()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateUserRequest { Id = Guid.NewGuid() };

        var result = await controller.UpdateUser(id, request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid update request, when UpdateUser called, then returns Ok")]
    public async Task Given_ValidUpdateRequest_When_UpdateUser_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        var request = new UpdateUserRequest { Id = id, Name = "Updated Name", Email =  "email@email.com", IsActive = true };

        var appResult = new UpdateUserResult { Id = id };
        var apiResponse = new UpdateUserResponse { Id = id };

        _mapper.Map<UpdateUserCommand>(request).Returns(new UpdateUserCommand());
        _mediator.Send(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(appResult));
        _mapper.Map<UpdateUserResponse>(appResult).Returns(apiResponse);

        var result = await controller.UpdateUser(id, request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<UpdateUserCommand>(request);
        await _mediator.Received(1).Send(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UpdateUserResponse>(appResult);
    }

    #endregion

    #region DeleteUser

    [Fact(DisplayName = "Given invalid id, when DeleteUser called, then returns BadRequest")]
    public async Task Given_InvalidId_When_DeleteUser_Then_BadRequest()
    {
        var controller = CreateController();

        var result = await controller.DeleteUser(Guid.Empty, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.NotNull(badRequest.Value);
    }

    [Fact(DisplayName = "Given valid id, when DeleteUser called, then returns Ok")]
    public async Task Given_ValidId_When_DeleteUser_Then_Ok()
    {
        var controller = CreateController();

        var id = Guid.NewGuid();
        _mapper.Map<DeleteUserCommand>(id).Returns(new DeleteUserCommand());
        _mediator.Send(Arg.Any<DeleteUserCommand>(), Arg.Any<CancellationToken>()).Returns(new DeleteUserResponse { Success = true });

        var result = await controller.DeleteUser(id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value);

        _mapper.Received(1).Map<DeleteUserCommand>(id);
        await _mediator.Received(1).Send(Arg.Any<DeleteUserCommand>(), Arg.Any<CancellationToken>());
    }

    #endregion
}