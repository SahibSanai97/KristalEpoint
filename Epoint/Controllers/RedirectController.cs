using Microsoft.AspNetCore.Mvc;

namespace Epoint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedirectController : ControllerBase
{
    /// <summary>
    /// success_url — Epoint redirects the user here after successful payment.
    /// Returns a default success response (replace with your mobile deep link later).
    /// </summary>
    [HttpGet("success")]
    public IActionResult Success([FromQuery] string? order_id, [FromQuery] string? transaction)
    {
        return Ok(new
        {
            status = "success",
            message = "Payment completed successfully",
            order_id = order_id ?? "test-order-001",
            transaction = transaction ?? "tw0000000101",
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// error_url — Epoint redirects the user here after failed payment.
    /// Returns a default error response.
    /// </summary>
    [HttpGet("error")]
    public IActionResult Error([FromQuery] string? order_id, [FromQuery] string? transaction)
    {
        return Ok(new
        {
            status = "error",
            message = "Payment failed or was cancelled",
            order_id = order_id ?? "test-order-001",
            transaction = transaction ?? "tw0000000101",
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// result_url — Epoint POSTs payment result here (server-to-server).
    /// Accepts data + signature from Epoint and returns 200 OK.
    /// </summary>
    [HttpPost("result")]
    public IActionResult Result([FromForm] string? data, [FromForm] string? signature)
    {
        // Log received data (in production, verify signature and update your DB here)
        return Ok(new
        {
            status = "received",
            message = "Payment result received successfully",
            data_received = !string.IsNullOrEmpty(data),
            signature_received = !string.IsNullOrEmpty(signature),
            timestamp = DateTime.UtcNow
        });
    }
}
