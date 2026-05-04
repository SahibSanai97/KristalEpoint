using Epoint.Helpers;
using Epoint.Models;
using Epoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PaymentController : ControllerBase
{
    private readonly IEpointService _epoint;

    public PaymentController(IEpointService epoint) => _epoint = epoint;

    /// <summary>Create a new payment and get a redirect URL.</summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PaymentRequest request)
    {
        var result = await _epoint.CreatePaymentAsync(request);
        return Ok(result);
    }

    /// <summary>Check the status of an existing payment by transaction ID.</summary>
    [HttpPost("status")]
    public async Task<IActionResult> Status([FromBody] GetStatusRequest request)
    {
        var result = await _epoint.GetPaymentStatusAsync(request);
        return Ok(result);
    }

    /// <summary>Register a card for future payments without entering card details each time.</summary>
    [HttpPost("register-card")]
    public async Task<IActionResult> RegisterCard([FromBody] CardRegistrationRequest request)
    {
        var result = await _epoint.RegisterCardAsync(request);
        return Ok(result);
    }

    /// <summary>Register a card and charge the first payment simultaneously.</summary>
    [HttpPost("register-card-with-pay")]
    public async Task<IActionResult> RegisterCardWithPay([FromBody] CardRegistrationWithPayRequest request)
    {
        var result = await _epoint.RegisterCardWithPayAsync(request);
        return Ok(result);
    }

    /// <summary>Charge a previously saved card by card_id.</summary>
    [HttpPost("execute-pay")]
    public async Task<IActionResult> ExecutePay([FromBody] ExecutePayRequest request)
    {
        var result = await _epoint.ExecutePayAsync(request);
        return Ok(result);
    }

    /// <summary>Transfer funds to a saved card (payout / refund to card).</summary>
    [HttpPost("refund")]
    public async Task<IActionResult> Refund([FromBody] RefundRequest request)
    {
        var result = await _epoint.RefundAsync(request);
        return Ok(result);
    }

    /// <summary>Reverse (cancel) a transaction fully or partially.</summary>
    [HttpPost("reverse")]
    public async Task<IActionResult> Reverse([FromBody] ReverseRequest request)
    {
        var result = await _epoint.ReverseAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Callback endpoint for Epoint to POST payment results.
    /// Epoint calls your result_url with data + signature.
    /// </summary>
    [HttpPost("callback")]
    public IActionResult Callback([FromForm] string data, [FromForm] string signature)
    {
        if (!_epoint.VerifyCallback(data, signature))
            return BadRequest(new { error = "Invalid signature" });

        var result = EpointSignatureHelper.DecodeData<EpointApiResponse>(data);
        // TODO: update your order in the database based on result.Status / result.OrderId
        return Ok(new { received = true, status = result?.Status });
    }
}
