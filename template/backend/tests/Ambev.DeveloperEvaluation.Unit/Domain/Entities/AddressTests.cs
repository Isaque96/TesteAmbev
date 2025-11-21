using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class AddressTests
{
    [Fact(DisplayName = "Address properties should be set and retrieved correctly")]
    public void Given_AddressProperties_When_Set_Then_PropertiesAreCorrect()
    {
        // Arrange
        var address = new Address
        {
            // Act
            City = "Sample City",
            Street = "Sample Street",
            Number = 123,
            ZipCode = "12345-678",
            Geolocation = new Geolocation { Lat = "10.0", Long = "20.0" }
        };

        // Assert
        Assert.Equal("Sample City", address.City);
        Assert.Equal("Sample Street", address.Street);
        Assert.Equal(123, address.Number);
        Assert.Equal("12345-678", address.ZipCode);
        Assert.NotNull(address.Geolocation);
        Assert.Equal("10.0", address.Geolocation.Lat);
        Assert.Equal("20.0", address.Geolocation.Long);
    }
}