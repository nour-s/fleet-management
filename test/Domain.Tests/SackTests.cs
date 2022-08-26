using Domain.Exceptions;

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

    [Fact]
    public void Sack_Can_Add_Package()
    {
        // Arrange
        var sack = new AutoFaker<Sack>().Generate();
        var package = new AutoFaker<Package>().Generate();

        // Act
        sack.AddPackage(package);

        // Assert
        sack.Packages.Single(x => x.Barcode == package.Barcode);
    }

    [Fact]
    public void Sack_Should_Not_Add_Null_Package()
    {
        // Arrange
        var sack = new AutoFaker<Sack>().Generate();

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => sack.AddPackage(null!));
    }

    [Fact]
    public void Sack_Should_Have_Created_State_When_First_Initialized()
    {
        // Arrange
        var sack = new AutoFaker<Sack>().Configure(x => x.WithSkip<SackState>()).Generate();
        var expectedState = SackState.Created;

        // Act
        var actualState = sack.State;

        // Assert
        Assert.Equal(expectedState, actualState);
    }

    [Fact]
    public void Sack_Unload_Should_First_Unload_All_Packages()
    {
        // Arrange
        var destination = DeliveryPointType.DistributionCentre;
        var sack = new AutoFaker<Sack>()
            .Configure(x => x.WithSkip<SackState>())
            .RuleFor(x => x.DeliveryPointType, destination)
            .Generate();

        var packageFaker = new AutoFaker<Package>()
            .Configure(x => x.WithSkip<PackageState>())
            .RuleFor(x => x.DeliveryPointType, destination);

        // Add random packages
        sack.AddPackage(packageFaker.Generate());
        sack.AddPackage(packageFaker.Generate());

        // Act
        sack.Unload(destination);

        // Assert
        Assert.All(sack.Packages, x => Assert.Equal(PackageState.Unloaded, x.State));
    }

    [Fact]
    public void Sack_Should_Not_Unload_To_Wrong_Delivery_Point()
    {
        // Arrange
        var sack = new AutoFaker<Sack>()
            .Configure(x => x.WithSkip<SackState>())
            .RuleFor(x => x.DeliveryPointType, DeliveryPointType.TransferCentre)
            .Generate();

        var wrongDeliveryPoint = DeliveryPointType.DistributionCentre;

        // Act && Assert
        Assert.Throws<DomainException>(() => sack.Unload(wrongDeliveryPoint));
    }

    [Fact]
    public void Sack_Should_Not_Unload_To_Branch()
    {
        // Arrange
        var sack = new AutoFaker<Sack>()
            .Configure(x => x.WithSkip<SackState>())
            .RuleFor(x => x.DeliveryPointType, DeliveryPointType.TransferCentre)
            .Generate();

        var wrongDeliveryPoint = DeliveryPointType.Branch;

        // Act && Assert
        Assert.Throws<DomainException>(() => sack.Unload(wrongDeliveryPoint));
    }

    // test if the sack gets unloaded when all packages of the sack are unloaded 
    [Fact]
    public void Sack_Should_Be_Unloaded_When_All_Packages_Are_Unloaded()
    {
        // Arrange
        var destination = DeliveryPointType.TransferCentre;
        var sack = new AutoFaker<Sack>()
            .Configure(x => x.WithSkip<SackState>())
            .RuleFor(x => x.DeliveryPointType, destination)
            .Generate();

        var packageFaker = new AutoFaker<Package>()
            .Configure(x => x.WithSkip<PackageState>())
            .RuleFor(x => x.DeliveryPointType, destination)
            .RuleFor(x => x.Sack, sack);

        // Add random packages
        sack.AddPackage(packageFaker.Generate());
        sack.AddPackage(packageFaker.Generate());

        // Act
        sack.Packages.ToList().ForEach(x => x.Unload(destination));

        // Assert
        Assert.Equal(SackState.Unloaded, sack.State);
    }
}
