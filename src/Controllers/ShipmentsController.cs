using Microsoft.AspNetCore.Mvc;

namespace WebApi;

[ApiController]
[Route("[controller]")]
public class ShipmentsController : Controller
{
    [HttpGet("deliver")]
    public IActionResult Deliver()
    {
        return Ok();
    }
}
