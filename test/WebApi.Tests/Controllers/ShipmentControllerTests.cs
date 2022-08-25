using MediatR;
using Moq;
using Application.Commands;
using System.Collections;
using KellermanSoftware.CompareNetObjects;
using Application.Queries;

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
    public async Task Deliver_Returns_OkResult_When_Valid_Request_Recieved()
    {
        // Arrange
        var fakeRequest = new AutoFaker<Delivery>().Generate();
        _mediator.Setup(x => x.Send(It.IsAny<DeliverShipmentsCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);
        _mediator.Setup(x => x.Send(It.IsAny<GetDeliveryStatusQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeRequest);

        // Act
        var result = await _sut.Deliver(fakeRequest);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var comparer = new CompareLogic(new ComparisonConfig());

        _mediator.Verify(x => x.Publish(It.Is<DeliverShipmentsCommand>(x => comparer.Compare(x.Delivery, fakeRequest).AreEqual), It.IsAny<CancellationToken>()), Times.Once);
    }
}