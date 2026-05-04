using System.Text.Json;
using System.Text.Json.Serialization;
using Epoint.Helpers;
using Epoint.Models;
using Microsoft.Extensions.Options;

namespace Epoint.Services;

public interface IEpointService
{
    Task<EpointApiResponse> CreatePaymentAsync(PaymentRequest request);
    Task<EpointApiResponse> GetPaymentStatusAsync(GetStatusRequest request);
    Task<EpointApiResponse> RegisterCardAsync(CardRegistrationRequest request);
    Task<EpointApiResponse> RegisterCardWithPayAsync(CardRegistrationWithPayRequest request);
    Task<EpointApiResponse> ExecutePayAsync(ExecutePayRequest request);
    Task<EpointApiResponse> RefundAsync(RefundRequest request);
    Task<EpointApiResponse> ReverseAsync(ReverseRequest request);
    Task<EpointApiResponse> SplitPaymentAsync(SplitPaymentRequest request);
    Task<EpointApiResponse> SplitExecutePayAsync(SplitExecutePayRequest request);
    Task<EpointApiResponse> SplitCardRegistrationWithPayAsync(SplitPaymentRequest request);
    Task<EpointApiResponse> PreauthRequestAsync(PreauthRequest request);
    Task<EpointApiResponse> PreauthCompleteAsync(PreauthCompleteRequest request);
    Task<EpointApiResponse> GetWidgetUrlAsync(WidgetRequest request);
    Task<EpointApiResponse> GetWalletStatusAsync();
    Task<EpointApiResponse> WalletPaymentAsync(WalletPaymentRequest request);
    Task<EpointApiResponse> CreateInvoiceAsync(InvoiceCreateRequest request);
    Task<EpointApiResponse> UpdateInvoiceAsync(InvoiceUpdateRequest request);
    Task<EpointApiResponse> ViewInvoiceAsync(int id);
    Task<EpointApiResponse> ListInvoicesAsync(string? type = null, string? order = null);
    Task<EpointApiResponse> SendInvoiceSmsAsync(int id, string phone);
    Task<EpointApiResponse> SendInvoiceEmailAsync(int id, string email);
    Task<EpointApiResponse> HeartbeatAsync();
    bool VerifyCallback(string data, string signature);
}

public class EpointService : IEpointService
{
    private readonly HttpClient _http;
    private readonly EpointSettings _settings;

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public EpointService(HttpClient http, IOptions<EpointSettings> settings)
    {
        _http = http;
        _settings = settings.Value;
    }

    // ─── helpers ───────────────────────────────────────────────────────────

    private (string data, string signature) Sign(object payload)
    {
        // Merge public_key into payload dynamically
        var dict = ToDict(payload);
        dict["public_key"] = _settings.PublicKey;

        string data = EpointSignatureHelper.BuildData(dict);
        string sig = EpointSignatureHelper.BuildSignature(_settings.PrivateKey, data);
        return (data, sig);
    }

    private static Dictionary<string, object?> ToDict(object obj)
    {
        // Serialize then deserialize to Dictionary to pick up all properties
        string json = JsonSerializer.Serialize(obj, _jsonOpts);
        return JsonSerializer.Deserialize<Dictionary<string, object?>>(json)!;
    }

    private async Task<EpointApiResponse> PostAsync(string endpoint, object payload)
    {
        var (data, sig) = Sign(payload);
        var form = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string,string>("data", data),
            new KeyValuePair<string,string>("signature", sig)
        });

        try
        {
            var resp = await _http.PostAsync($"{_settings.BaseUrl}/{endpoint.TrimStart('/')}", form);
            string body = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EpointApiResponse>(body, _jsonOpts)
                   ?? new EpointApiResponse { Error = "Empty response" };
        }
        catch (Exception ex)
        {
            return new EpointApiResponse { Error = ex.Message };
        }
    }

    // ─── public methods ────────────────────────────────────────────────────

    public Task<EpointApiResponse> CreatePaymentAsync(PaymentRequest r) =>
        PostAsync("request", r);

    public Task<EpointApiResponse> GetPaymentStatusAsync(GetStatusRequest r) =>
        PostAsync("get-status", r);

    public Task<EpointApiResponse> RegisterCardAsync(CardRegistrationRequest r) =>
        PostAsync("card-registration", r);

    public Task<EpointApiResponse> RegisterCardWithPayAsync(CardRegistrationWithPayRequest r) =>
        PostAsync("card-registration-with-pay", r);

    public Task<EpointApiResponse> ExecutePayAsync(ExecutePayRequest r) =>
        PostAsync("execute-pay", r);

    public Task<EpointApiResponse> RefundAsync(RefundRequest r) =>
        PostAsync("refund-request", r);

    public Task<EpointApiResponse> ReverseAsync(ReverseRequest r) =>
        PostAsync("reverse", r);

    public Task<EpointApiResponse> SplitPaymentAsync(SplitPaymentRequest r) =>
        PostAsync("split-request", r);

    public Task<EpointApiResponse> SplitExecutePayAsync(SplitExecutePayRequest r) =>
        PostAsync("split-execute-pay", r);

    public Task<EpointApiResponse> SplitCardRegistrationWithPayAsync(SplitPaymentRequest r) =>
        PostAsync("split-card-registration-with-pay", r);

    public Task<EpointApiResponse> PreauthRequestAsync(PreauthRequest r) =>
        PostAsync("pre-auth-request", r);

    public Task<EpointApiResponse> PreauthCompleteAsync(PreauthCompleteRequest r) =>
        PostAsync("pre-auth-complete", r);

    public Task<EpointApiResponse> GetWidgetUrlAsync(WidgetRequest r) =>
        PostAsync("token/widget", r);

    public Task<EpointApiResponse> GetWalletStatusAsync()
    {
        var payload = new { public_key = _settings.PublicKey };
        return PostAsync("wallet/status", payload);
    }

    public Task<EpointApiResponse> WalletPaymentAsync(WalletPaymentRequest r) =>
        PostAsync("wallet/payment", r);

    public Task<EpointApiResponse> CreateInvoiceAsync(InvoiceCreateRequest r) =>
        PostAsync("invoices/create", r);

    public Task<EpointApiResponse> UpdateInvoiceAsync(InvoiceUpdateRequest r) =>
        PostAsync("invoices/update", r);

    public Task<EpointApiResponse> ViewInvoiceAsync(int id) =>
        PostAsync("invoices/view", new { id });

    public Task<EpointApiResponse> ListInvoicesAsync(string? type = null, string? order = null) =>
        PostAsync("invoices/list", new { type, order });

    public Task<EpointApiResponse> SendInvoiceSmsAsync(int id, string phone) =>
        PostAsync("invoices/send-sms", new { id, phone });

    public Task<EpointApiResponse> SendInvoiceEmailAsync(int id, string email) =>
        PostAsync("invoices/send-email", new { id, email });

    public async Task<EpointApiResponse> HeartbeatAsync()
    {
        try
        {
            var resp = await _http.GetAsync($"{_settings.BaseUrl.Replace("/api/1", "")}/api/heartbeat");
            string body = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EpointApiResponse>(body, _jsonOpts)
                   ?? new EpointApiResponse { Error = "Empty response" };
        }
        catch (Exception ex)
        {
            return new EpointApiResponse { Error = ex.Message };
        }
    }

    public bool VerifyCallback(string data, string signature) =>
        EpointSignatureHelper.VerifySignature(_settings.PrivateKey, data, signature);
}
