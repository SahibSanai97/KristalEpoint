using Epoint.Models;
using Epoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WidgetController : ControllerBase
{
    private readonly IEpointService _epoint;

    public WidgetController(IEpointService epoint) => _epoint = epoint;

    /// <summary>
    /// Get a widget URL for Apple Pay or Google Pay.
    /// Embed the returned widget_url in an iframe or webview.
    /// </summary>
    [HttpPost("url")]
    public async Task<IActionResult> GetUrl([FromBody] WidgetRequest request)
    {
        var result = await _epoint.GetWidgetUrlAsync(request);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HeartbeatController : ControllerBase
{
    private readonly IEpointService _epoint;

    public HeartbeatController(IEpointService epoint) => _epoint = epoint;

    /// <summary>Check Epoint service availability.</summary>
    [HttpGet]
    public async Task<IActionResult> Check()
    {
        var result = await _epoint.HeartbeatAsync();
        return Ok(result);
    }
}
