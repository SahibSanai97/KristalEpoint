using Epoint.Models;
using Epoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PreauthController : ControllerBase
{
    private readonly IEpointService _epoint;

    public PreauthController(IEpointService epoint) => _epoint = epoint;

    /// <summary>
    /// Initiate a pre-authorization. Funds are held but not captured
    /// until you call /complete.
    /// </summary>
    [HttpPost("request")]
    public async Task<IActionResult> Request([FromBody] PreauthRequest request)
    {
        var result = await _epoint.PreauthRequestAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Complete a previously initiated pre-authorization.
    /// Must be called after the transaction is processed by Epoint.
    /// </summary>
    [HttpPost("complete")]
    public async Task<IActionResult> Complete([FromBody] PreauthCompleteRequest request)
    {
        var result = await _epoint.PreauthCompleteAsync(request);
        return Ok(result);
    }
}
