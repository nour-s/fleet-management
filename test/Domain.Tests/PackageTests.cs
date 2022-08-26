using Domain.Exceptions;

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
        .Configure(x => x.WithSkip<PackageState>().WithSkip<Sack>())
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
        Assert.Throws<DomainException>(() => package.Unload(wrongDeliveryPoint));
    }

    // Shipment should not be unloaded to a branch if there is a sack
    [Fact]
    public void Package_Should_Not_Unload_To_Branch_If_It_Is_In_Sack()
    {
        // Arrange
        var package = new Package("12345", DeliveryPointType.Branch, 1) { Sack = new Sack("234", DeliveryPointType.Branch) };

        // Act and Assert
        Assert.Throws<DomainException>(() => package.Unload(DeliveryPointType.Branch));
    }


    // Shipment should not be unloaded to a branch if there is a sack
    [Fact]
    public void Package_Should_Not_Unload_To_TransferCentre_If_It_Is_Not_In_Sack()
    {
        // Arrange
        var package = new Package("12345", DeliveryPointType.TransferCentre, 1) { Sack = null };

        // Act and Assert
        Assert.Throws<DomainException>(() => package.Unload(DeliveryPointType.TransferCentre));
    }
}