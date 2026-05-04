using Epoint.Models;
using Epoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InvoiceController : ControllerBase
{
    private readonly IEpointService _epoint;

    public InvoiceController(IEpointService epoint) => _epoint = epoint;

    /// <summary>Create a new invoice.</summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] InvoiceCreateRequest request)
    {
        var result = await _epoint.CreateInvoiceAsync(request);
        return Ok(result);
    }

    /// <summary>Update an existing invoice.</summary>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] InvoiceUpdateRequest request)
    {
        var result = await _epoint.UpdateInvoiceAsync(request);
        return Ok(result);
    }

    /// <summary>View details of a specific invoice.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> View(int id)
    {
        var result = await _epoint.ViewInvoiceAsync(id);
        return Ok(result);
    }

    /// <summary>List all invoices, optionally filtered by type and sorted.</summary>
    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] string? type, [FromQuery] string? order)
    {
        var result = await _epoint.ListInvoicesAsync(type, order);
        return Ok(result);
    }

    /// <summary>Send invoice link to a customer via SMS.</summary>
    [HttpPost("{id}/send-sms")]
    public async Task<IActionResult> SendSms(int id, [FromQuery] string phone)
    {
        var result = await _epoint.SendInvoiceSmsAsync(id, phone);
        return Ok(result);
    }

    /// <summary>Send invoice link to a customer via email.</summary>
    [HttpPost("{id}/send-email")]
    public async Task<IActionResult> SendEmail(int id, [FromQuery] string email)
    {
        var result = await _epoint.SendInvoiceEmailAsync(id, email);
        return Ok(result);
    }
}
