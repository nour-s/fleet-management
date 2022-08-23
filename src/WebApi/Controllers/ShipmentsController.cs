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
    public IActionResult Deliver([FromBody] Delivery request)
    {
        return Ok();
    }
}
