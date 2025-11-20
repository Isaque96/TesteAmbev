namespace Ambev.DeveloperEvaluation.Application.Categories.ListProductsByCategory;

public class ListProductsByCategoryResult
{
    public IEnumerable<ListProductsByCategoryItem> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class ListProductsByCategoryItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int Count { get; set; }
}