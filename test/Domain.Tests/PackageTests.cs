namespace Domain.Tests;

public class PackageTest
{
    [Fact]
    public void Package_Can_Be_Initialized_With_Correct_Properties()
    {
        var package = new Package("12345", DeliveryPointType.Branch, 1);

        Assert.Equal("12345", package.Barcode);
        Assert.Equal(DeliveryPointType.Branch, package.DeliveryPointType);
        Assert.Equal(1, package.Desi);
    }
}