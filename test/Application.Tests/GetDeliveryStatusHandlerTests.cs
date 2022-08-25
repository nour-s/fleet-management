using Application.Persistence;
using Domain.Models;
using WebApi.Models;
using Moq;
using System.Linq.Expressions;
using Bogus;
using Application.Queries;
using AutoBogus;

namespace Application.Tests;

public class GetDeliveryStatusHandlerTests
{
    private readonly GetDeliveryStatusHandler _sut;

    // mock sack repo
    private readonly Mock<IRepository<Sack>> _sackRepositoryMock = new();
    private readonly Mock<IRepository<Package>> _packageRepositoryMock = new();

    public GetDeliveryStatusHandlerTests()
    {
        _sut = new GetDeliveryStatusHandler(
            _sackRepositoryMock.Object,
            _packageRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Process_A_Shipment_Of_Type_Package()
    {
        // Arrange
        var selectedDeliveryPoint = DeliveryPointType.Branch;
        var packageBarcorde = new Randomizer().Replace("P##########");
        var packageState = new Randomizer().Enum<PackageState>();

        var package = new Package(packageBarcorde, selectedDeliveryPoint, new Random().Next()) { State = packageState };

        var delivery = new Delivery
        {
            Routes = new List<Route>
            {
                new Route
                {
                    DeliveryPoint = (int)selectedDeliveryPoint,
                    Deliveries = new List<Shipment> { new Shipment { Barcode = packageBarcorde } }
                }
            }
        };

        _packageRepositoryMock
            .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Package, bool>>>(), It.IsAny<Expression<Func<Package, object>>>()))
                .ReturnsAsync(package);

        var query = new GetDeliveryStatus(delivery);

        // Act

        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert

        // verify that package is fetched from db.
        _packageRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Package, bool>>>(), It.IsAny<Expression<Func<Package, object>>[]>()), Times.Once);

        // Assert that the package is unloaded
        Assert.Equal((int)packageState, result.Routes.First().Deliveries.First().State);
    }


    [Fact]
    public async Task Handle_Should_Process_A_Shipment_Of_Type_Sack()
    {
        // Arrange
        var selectedDeliveryPoint = DeliveryPointType.DistributionCentre;
        var sackBarcorde = new Randomizer().Replace("C##########");
        var sackState = new Randomizer().Enum<SackState>();

        var sack = new Sack(sackBarcorde, selectedDeliveryPoint) { State = sackState };

        var delivery = new Delivery
        {
            Routes = new List<Route>
            {
                new Route
                {
                    DeliveryPoint = (int)selectedDeliveryPoint,
                    Deliveries = new List<Shipment> { new Shipment { Barcode = sackBarcorde } }
                }
            }
        };

        _sackRepositoryMock
            .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Sack, bool>>>()))
                .ReturnsAsync(sack);

        var query = new GetDeliveryStatus(delivery);

        // Act

        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert

        // verify that sack is fetched from db.
        _sackRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Sack, bool>>>()), Times.Once);

        // Assert that the sack is unloaded
        Assert.Equal((int)sackState, result.Routes.First().Deliveries.First().State);
    }

}
