using Domain;

namespace Domain.Tests;

public class SackTests
{
    [Fact]
    public void Sack_Can_Be_Initialized_With_Correct_Properties()
    {
        var sack = new Sack("12345", DeliveryPointType.Branch);
        Assert.Equal("12345", sack.Barcode);
        Assert.Equal(DeliveryPointType.Branch, sack.DeliveryPointType);
    }
}