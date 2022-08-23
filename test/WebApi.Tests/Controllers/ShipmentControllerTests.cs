

using MediatR;
using Moq;

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
    }
}