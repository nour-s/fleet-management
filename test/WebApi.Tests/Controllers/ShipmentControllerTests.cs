using MediatR;
using Moq;
using Application.Commands;
using System.Collections;
using KellermanSoftware.CompareNetObjects;

namespace WebApi.Tests.Controllers;

public class ShipmentsControllerTests
{
    /// <summary>
    /// System under test (controller)
    /// </summary>
    private readonly ShipmentsController _sut;
    private readonly Mock<IMediator> _mediator;

    public ShipmentsControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _sut = new ShipmentsController(_mediator.Object);
    }

    [Fact]
    public void Deliver_Returns_OkResult_When_Valid_Request_Recieved()
    {
        // Arrange
        var fakeRequest = new AutoFaker<Delivery>().Generate();

        // Act
        var result = _sut.Deliver(fakeRequest);

        // Assert
        Assert.IsType<OkResult>(result);
        var comparer = new CompareLogic(new ComparisonConfig());

        _mediator.Verify(x => x.Publish(It.Is<DeliverShipmentsCommand>(x => comparer.Compare(x.Delivery, fakeRequest).AreEqual), It.IsAny<CancellationToken>()), Times.Once);
    }
}