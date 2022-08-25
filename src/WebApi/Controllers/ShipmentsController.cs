using Application.Commands;
using Application.Queries;
using MediatR;

namespace WebApi;

[ApiController]
[Route("[controller]")]
public class ShipmentsController : Controller
{
    private IMediator _mediator;

    public ShipmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("deliver")]
    public async Task<IActionResult> Deliver([FromBody] Delivery request)
    {
        await _mediator.Publish(new DeliverShipmentsCommand(request));
        var deliveryStatus = await _mediator.Send(new GetDeliveryStatusQuery(request));
        return Ok(deliveryStatus);
    }
}
