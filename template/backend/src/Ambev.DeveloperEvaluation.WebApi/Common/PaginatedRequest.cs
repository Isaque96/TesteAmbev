using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class PaginatedRequest
{
    /// <summary>
    /// Page number (default: 1)
    /// </summary>
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Page size (default: 10)
    /// </summary>
    [FromQuery(Name = "_size")]
    public int Size   { get; set; } = 10;

    /// <summary>
    /// Order by (e.g., "date desc")
    /// </summary>
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }
}