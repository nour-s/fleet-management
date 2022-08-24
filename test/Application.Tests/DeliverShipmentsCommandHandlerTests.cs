using Application.Commands;
using WebApi.Models;

namespace Application.Tests;

public class DeliverShipmentsCommandHandlerTests
{
    private readonly DeliverShipmentsCommandHandler _sut;

    public DeliverShipmentsCommandHandlerTests()
    {
        _sut = new DeliverShipmentsCommandHandler();
    }

    [Fact]
    public void Handle_Should_Query_Database_For_Every_Shipment()
    {
        var command = new DeliverShipmentsCommand(new Delivery());

        _sut.Handle(command, CancellationToken.None);
    }
}