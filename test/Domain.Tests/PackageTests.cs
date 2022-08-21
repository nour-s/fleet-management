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

    [Fact]
    public void Package_Can_Load_Into_Sack()
    {
        // Arrange
        var package = new AutoFaker<Package>().Generate();

        // Act
        package.Load();

        // Assert

        // clone the original sack so we make sure it is not a reference comparison.
        Assert.Equal(PackageState.LoadedInSack, package.State);
    }

    [Fact]
    public void Package_Can_Unload()
    {
        var deliveryPoint = DeliveryPointType.Branch;

        // Arrange
        var package = new AutoFaker<Package>()
        .Configure(x => x.WithSkip<PackageState>())
        .RuleFor(x => x.DeliveryPointType, deliveryPoint)
        .Generate();

        // Act
        package.Unload(deliveryPoint);

        // Assert
        Assert.Equal(PackageState.Unloaded, package.State);
    }

    [Fact]
    public void Package_Should_Not_Unload_To_Wrong_Delivery_Point()
    {
        // Arrange
        var package = new Package("12345", DeliveryPointType.Branch, 1);
        var wrongDeliveryPoint = DeliveryPointType.DistributionCentre;

        // Act && Assert
        Assert.Throws<ArgumentException>(() => package.Unload(wrongDeliveryPoint));
    }

}