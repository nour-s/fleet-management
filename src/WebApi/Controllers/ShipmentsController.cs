namespace WebApi;

[ApiController]
[Route("[controller]")]
public class ShipmentsController : Controller
{
    [HttpPost("deliver")]
    public IActionResult Deliver([FromBody] Delivery request)
    {
        return Ok();
    }
}
