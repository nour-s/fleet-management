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
        await _mediator.Send(new DeliverShipmentsCommand(request));
        var deliveryStatus = await _mediator.Send(new GetDeliveryStatusQuery(request));
        return Ok(deliveryStatus);
    }

    [HttpGet]
    public async Task<IActionResult> Deliver([FromQuery] string barcode)
    {
        var result = await _mediator.Send(new GetShipmentStatusQuery(barcode));
        return Ok(new { Barcode = barcode, Status = result });
    }

}
