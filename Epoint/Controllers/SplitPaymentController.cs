using Epoint.Models;
using Epoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SplitPaymentController : ControllerBase
{
    private readonly IEpointService _epoint;

    public SplitPaymentController(IEpointService epoint) => _epoint = epoint;

    /// <summary>Create a split payment between two users.</summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] SplitPaymentRequest request)
    {
        var result = await _epoint.SplitPaymentAsync(request);
        return Ok(result);
    }

    /// <summary>Execute a split payment using a saved card.</summary>
    [HttpPost("execute-pay")]
    public async Task<IActionResult> ExecutePay([FromBody] SplitExecutePayRequest request)
    {
        var result = await _epoint.SplitExecutePayAsync(request);
        return Ok(result);
    }

    /// <summary>Register a card and perform a split payment at the same time.</summary>
    [HttpPost("register-card-with-pay")]
    public async Task<IActionResult> RegisterCardWithPay([FromBody] SplitPaymentRequest request)
    {
        var result = await _epoint.SplitCardRegistrationWithPayAsync(request);
        return Ok(result);
    }
}
