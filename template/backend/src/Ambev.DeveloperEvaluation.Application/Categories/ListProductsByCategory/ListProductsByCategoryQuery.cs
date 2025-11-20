using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.ListProductsByCategory;

public class ListProductsByCategoryQuery : IRequest<ListProductsByCategoryResult>
{
    public string CategoryName { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
}