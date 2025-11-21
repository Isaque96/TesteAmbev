using AutoMapper.Configuration.Annotations;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a user's address.
/// </summary>
public class Address
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string ZipCode { get; set; } = string.Empty;
    [Ignore]
    public Geolocation Geolocation { get; set; } = new();
}