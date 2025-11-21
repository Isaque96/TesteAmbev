using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Commons;

public class ValidationResultDetailTests
{
    [Fact(DisplayName = "Given parameterless constructor, when instance created, then IsValid default is false and Errors is empty")]
    public void Given_ParameterlessConstructor_When_InstanceCreated_Then_DefaultValues()
    {
        // Act
        var resultDetail = new ValidationResultDetail();

        // Assert
        Assert.False(resultDetail.IsValid); // bool default é false
        Assert.NotNull(resultDetail.Errors);
        Assert.Empty(resultDetail.Errors);
    }

    [Fact(DisplayName = "Given valid ValidationResult, when constructed, then IsValid is true and Errors is empty")]
    public void Given_ValidValidationResult_When_Constructed_Then_IsValidTrueAndNoErrors()
    {
        // Arrange
        var fluentResult = new ValidationResult(); // sem falhas => IsValid = true

        // Act
        var resultDetail = new ValidationResultDetail(fluentResult);

        // Assert
        Assert.True(resultDetail.IsValid);
        Assert.NotNull(resultDetail.Errors);
        Assert.Empty(resultDetail.Errors);
    }

    [Fact(DisplayName = "Given invalid ValidationResult, when constructed, then IsValid is false and Errors are mapped")]
    public void Given_InvalidValidationResult_When_Constructed_Then_IsValidFalseAndErrorsMapped()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Age", "Age must be greater than 18")
        };

        var fluentResult = new ValidationResult(failures);

        // Act
        var resultDetail = new ValidationResultDetail(fluentResult);

        // Assert
        Assert.False(resultDetail.IsValid);
        Assert.NotNull(resultDetail.Errors);

        var errorsList = resultDetail.Errors.ToList();
        Assert.Equal(2, errorsList.Count);
    }
}