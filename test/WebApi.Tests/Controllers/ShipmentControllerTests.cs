using Microsoft.AspNetCore.Mvc;

namespace WebApi.Tests.Controllers;

public class ShipmentsControllerTests
{
    [Fact]
    public void Deliver_Returns_OkResult_When_Valid_Request_Recieved()
    {
        // Arrange
        var controller = new ShipmentsController();

        // Act
        var result = controller.Deliver();

        // Assert
        Assert.IsType<OkResult>(result);
    }
}