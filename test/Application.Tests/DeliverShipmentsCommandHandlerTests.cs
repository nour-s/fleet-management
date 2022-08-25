using Application.Commands;
using Application.Persistence;
using Domain.Models;
using WebApi.Models;
using Moq;
using System.Linq.Expressions;
using Bogus;

namespace Application.Tests;

public class DeliverShipmentsCommandHandlerTests
{
    private readonly DeliverShipmentsCommandHandler _sut;

    // mock sack repo
    private readonly Mock<IRepository<Sack>> _sackRepositoryMock = new();
    private readonly Mock<IRepository<Package>> _packageRepositoryMock = new();

    public DeliverShipmentsCommandHandlerTests()
    {
        _sut = new DeliverShipmentsCommandHandler(
            _sackRepositoryMock.Object,
            _packageRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Process_A_Shipment_Of_Type_Package()
    {
        // Arrange
        var selectedDeliveryPoint = DeliveryPointType.Branch;
        var packageBarcode = new Randomizer().Replace("P##########");

        var package = new Package(packageBarcode, selectedDeliveryPoint, new Random().Next());

        var delivery = new Delivery
        {
            Routes = new List<Route>
            {
                new Route
                {
                    DeliveryPoint = (int)selectedDeliveryPoint,
                    Deliveries = new List<Shipment> { new Shipment { Barcode = packageBarcode } }
                }
            }
        };

        _packageRepositoryMock
            .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Package, bool>>>(), It.IsAny<Expression<Func<Package, object>>>()))
                .ReturnsAsync(package);

        var command = new DeliverShipmentsCommand(delivery);

        // Act

        await _sut.Handle(command, CancellationToken.None);

        // Assert

        // verify that package is fetched from db.
        _packageRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Package, bool>>>(), It.IsAny<Expression<Func<Package, object>>[]>()), Times.Once);
        _packageRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        // Assert that the package is unloaded
        Assert.Equal(PackageState.Unloaded, package.State);
    }


    [Fact]
    public async Task Handle_Should_Process_A_Shipment_Of_Type_Sack()
    {
        // Arrange
        var selectedDeliveryPoint = DeliveryPointType.Branch;
        var sackBarcode = new Randomizer().Replace("C##########");

        var sack = new Sack(sackBarcode, selectedDeliveryPoint);

        var delivery = new Delivery
        {
            Routes = new List<Route>
            {
                new Route
                {
                    DeliveryPoint = (int)selectedDeliveryPoint,
                    Deliveries = new List<Shipment> { new Shipment { Barcode = sackBarcode } }
                }
            }
        };

        _sackRepositoryMock
            .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Sack, bool>>>(), It.IsAny<Expression<Func<Sack, object>>>()))
                .ReturnsAsync(sack);

        var command = new DeliverShipmentsCommand(delivery);

        // Act

        await _sut.Handle(command, CancellationToken.None);

        // Assert

        // verify that package is fetched from db.
        _sackRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Sack, bool>>>(), It.IsAny<Expression<Func<Sack, object>>[]>()), Times.Once);
        _sackRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        // Assert that the package is unloaded
        Assert.Equal(SackState.Unloaded, sack.State);
    }
}
