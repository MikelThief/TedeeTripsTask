using FluentAssertions;
using TedeeTrips.Domain.ValueObjects;
using Xunit;

namespace TedeeTrips.Tests.Unit.Domain.ValueObjects;

public class TripNameTests
{
    [Theory]
    [InlineData("Trip to the moon", true)]
    [InlineData("Na koniec świata i jeszcze dalej!", true)]
    [InlineData("Trip to the infinity\r and beyond!", false)]
    [InlineData("Trip to the infinity\r\n and beyond!", false)]
    [InlineData("Trip to the infinity\n and beyond!", false)]
    public void Only_A_Single_Line_Is_Allowed(string tripName, bool expectedSuccess) => TripName.Create(tripName).IsSuccess.Should().Be(expectedSuccess);
}