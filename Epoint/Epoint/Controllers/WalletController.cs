using Epoint.Models;
using Epoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WalletController : ControllerBase
{
    private readonly IEpointService _epoint;

    public WalletController(IEpointService epoint) => _epoint = epoint;

    /// <summary>Retrieve the list of available wallets for the merchant.</summary>
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var result = await _epoint.GetWalletStatusAsync();
        return Ok(result);
    }

    /// <summary>Create a payment using a specific wallet.</summary>
    [HttpPost("payment")]
    public async Task<IActionResult> Payment([FromBody] WalletPaymentRequest request)
    {
        var result = await _epoint.WalletPaymentAsync(request);
        return Ok(result);
    }
}
