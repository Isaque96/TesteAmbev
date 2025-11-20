namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.DeleteCategory;

/// <summary>
/// Request model for deleting a category
/// </summary>
public class DeleteCategoryRequest
{
    /// <summary>
    /// CategoryName ID to delete
    /// </summary>
    public Guid Id { get; set; }
}