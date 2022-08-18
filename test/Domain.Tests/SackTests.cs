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
    public void Sack_Unload_Unload_All_Packages()
    {
        var sack = new AutoFaker<Sack>().Configure(x => x.WithSkip<SackState>()).Generate();

        // Act
        sack.Unload(DeliveryPointType.Branch);
    }
}
