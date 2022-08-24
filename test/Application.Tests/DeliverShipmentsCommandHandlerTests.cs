using Application.Commands;
using Application.Persistence;
using Domain.Models;
using WebApi.Models;
using Moq;

namespace Application.Tests;

public class DeliverShipmentsCommandHandlerTests
{
    private readonly DeliverShipmentsCommandHandler _sut;

    // mock sack repo
    private readonly Mock<IRepository<Sack>> _sackRepositoryMock = new();
    private readonly Mock<IRepository<Package>> _packageRepositoryMock = new();


    public DeliverShipmentsCommandHandlerTests()
    {
        _sut = new DeliverShipmentsCommandHandler(_sackRepositoryMock.Object, _packageRepositoryMock.Object);
    }

    [Fact]
    public void Handle_Should_Query_Database_For_Every_Shipment()
    {
        var command = new DeliverShipmentsCommand(new Delivery());

        _sut.Handle(command, CancellationToken.None);
    }
}