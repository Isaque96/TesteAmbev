namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents geolocation coordinates.
/// </summary>
public class Geolocation
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}