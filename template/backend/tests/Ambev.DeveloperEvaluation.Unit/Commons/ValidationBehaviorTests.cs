using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Commons;

public class ValidationBehaviorTests
{
    private record TestRequest(string Name) : IRequest<TestResponse>;
    private record TestResponse(string Message);

    [Fact(DisplayName = "Given no validators, when Handle called, then next delegate is executed")]
    public async Task Given_NoValidators_When_Handle_Then_NextIsExecuted()
    {
        // Arrange
        var validators = Array.Empty<IValidator<TestRequest>>();
        var behavior   = new ValidationBehavior<TestRequest, TestResponse>(validators);

        var expectedResponse = new TestResponse("ok");

        var callCount = 0;

        var request = new TestRequest("anything");
        var ct      = CancellationToken.None;

        // Act
        var response = await behavior.Handle(request, Next, ct);

        // Assert
        Assert.NotNull(expectedResponse.Message);
        Assert.NotNull(request.Name);
        Assert.Equal(expectedResponse, response);
        Assert.Equal(1, callCount);
        return;

        Task<TestResponse> Next()
        {
            callCount++;
            return Task.FromResult(expectedResponse);
        }
    }

    [Fact(DisplayName = "Given validators without failures, when Handle called, then next delegate is executed")]
    public async Task Given_ValidRequest_When_Handle_Then_NextIsExecuted()
    {
        // Arrange
        var validator = new FakeValidatorWithoutErrors();
        var validators = new[] { validator as IValidator<TestRequest> };
        var behavior   = new ValidationBehavior<TestRequest, TestResponse>(validators);

        var expectedResponse = new TestResponse("ok");

        var callCount = 0;

        var request = new TestRequest("valid name");
        var ct      = CancellationToken.None;

        // Act
        var response = await behavior.Handle(request, Next, ct);

        // Assert
        Assert.Equal(expectedResponse, response);
        Assert.Equal(1, callCount);
        Assert.Equal(1, validator.ValidateAsyncCallCount);
        return;

        Task<TestResponse> Next()
        {
            callCount++;
            return Task.FromResult(expectedResponse);
        }
    }

    [Fact(DisplayName = "Given validators with failures, when Handle called, then ValidationException is thrown and next is not executed")]
    public async Task Given_InvalidRequest_When_Handle_Then_ThrowsValidationException_And_DoesNotCallNext()
    {
        // Arrange
        var validator = new FakeValidatorWithFailure("Name", "Name is required");
        var validators = new[] { validator as IValidator<TestRequest> };
        var behavior   = new ValidationBehavior<TestRequest, TestResponse>(validators);

        var callCount = 0;

        var request = new TestRequest(string.Empty);
        var ct      = CancellationToken.None;

        // Act + Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(request, Next, ct));

        Assert.Contains(ex.Errors, e => e.PropertyName == "Name");
        Assert.Equal(0, callCount);
        Assert.Equal(1, validator.ValidateAsyncCallCount);
        return;

        Task<TestResponse> Next()
        {
            callCount++;
            return Task.FromResult(new TestResponse("should not be returned"));
        }
    }

    #region Fake validators

    private class FakeValidatorWithoutErrors : AbstractValidator<TestRequest>
    {
        public int ValidateAsyncCallCount { get; private set; }

        public override Task<ValidationResult> ValidateAsync(
            ValidationContext<TestRequest> context,
            CancellationToken cancellation = default)
        {
            ValidateAsyncCallCount++;
            return Task.FromResult(new ValidationResult());
        }
    }

    private class FakeValidatorWithFailure(string propertyName, string message) : AbstractValidator<TestRequest>
    {
        public int ValidateAsyncCallCount { get; private set; }

        public override Task<ValidationResult> ValidateAsync(
            ValidationContext<TestRequest> context,
            CancellationToken cancellation = default)
        {
            ValidateAsyncCallCount++;

            var failure = new ValidationFailure(propertyName, message);
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { failure }));
        }
    }

    #endregion
}