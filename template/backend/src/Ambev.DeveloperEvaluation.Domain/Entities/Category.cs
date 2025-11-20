using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Category : BaseEntity
{
    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to Products.
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public Category()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the category entity.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new CategoryValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}