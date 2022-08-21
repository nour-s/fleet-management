

namespace WebApi.Tests.Controllers;

public class ShipmentsControllerTests
{
    [Fact]
    public void Deliver_Returns_OkResult_When_Valid_Request_Recieved()
    {
        // Arrange
        var controller = new ShipmentsController();
        var fakeRequest = new AutoFaker<Delivery>().Generate();

        // Act
        var result = controller.Deliver(fakeRequest);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}