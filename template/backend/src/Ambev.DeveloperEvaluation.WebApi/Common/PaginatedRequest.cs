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

    private string _order = "ASC";
    /// <summary>
    /// Order by (e.g., "date desc")
    /// </summary>
    [FromQuery(Name = "_order")]
    public string Order
    {
        get => _order;
        set
        {
            var normalized = value?.Trim().ToUpper();

            // Se o projeto só quer aceitar ASC/DESC:
            _order = normalized is "ASC" or "DESC"
                ? normalized
                : "ASC";
        }
    }
}