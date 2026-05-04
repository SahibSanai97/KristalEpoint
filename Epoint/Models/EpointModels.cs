namespace Epoint.Models;

// ── Payment Request ──────────────────────────────────────────────
public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string Language { get; set; } = "az";
    public string OrderId { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? IsInstallment { get; set; }
    public string? SuccessRedirectUrl { get; set; }
    public string? ErrorRedirectUrl { get; set; }
    public Dictionary<string, string>? OtherAttr { get; set; }
}

// ── Card Registration Request ────────────────────────────────────
public class CardRegistrationRequest
{
    public string Language { get; set; } = "az";
    public int? Refund { get; set; }
    public string? Description { get; set; }
    public string? SuccessRedirectUrl { get; set; }
    public string? ErrorRedirectUrl { get; set; }
}

// ── Card Registration With Pay Request ──────────────────────────
public class CardRegistrationWithPayRequest
{
    public string Language { get; set; } = "az";
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string? Description { get; set; }
    public string? SuccessRedirectUrl { get; set; }
    public string? ErrorRedirectUrl { get; set; }
}

// ── Execute Pay (Saved Card) Request ────────────────────────────
public class ExecutePayRequest
{
    public string Language { get; set; } = "az";
    public string CardId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string? Description { get; set; }
}

// ── Refund Request ───────────────────────────────────────────────
public class RefundRequest
{
    public string Language { get; set; } = "az";
    public string CardId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string? Description { get; set; }
}

// ── Reverse (Cancel) Request ─────────────────────────────────────
public class ReverseRequest
{
    public string Language { get; set; } = "az";
    public string Transaction { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Currency { get; set; } = "AZN";
}

// ── Get Status Request ───────────────────────────────────────────
public class GetStatusRequest
{
    public string Transaction { get; set; } = string.Empty;
}

// ── Split Payment Request ────────────────────────────────────────
public class SplitPaymentRequest
{
    public decimal Amount { get; set; }
    public string SplitUser { get; set; } = string.Empty;
    public decimal SplitAmount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string Language { get; set; } = "az";
    public string OrderId { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SuccessRedirectUrl { get; set; }
    public string? ErrorRedirectUrl { get; set; }
    public Dictionary<string, string>? OtherAttr { get; set; }
}

// ── Split Execute Pay Request ────────────────────────────────────
public class SplitExecutePayRequest
{
    public string Language { get; set; } = "az";
    public string CardId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string SplitUser { get; set; } = string.Empty;
    public decimal SplitAmount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string? Description { get; set; }
}

// ── Preauth Request ──────────────────────────────────────────────
public class PreauthRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string Language { get; set; } = "az";
    public string OrderId { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SuccessRedirectUrl { get; set; }
    public string? ErrorRedirectUrl { get; set; }
    public Dictionary<string, string>? OtherAttr { get; set; }
}

// ── Preauth Complete Request ─────────────────────────────────────
public class PreauthCompleteRequest
{
    public decimal Amount { get; set; }
    public string Transaction { get; set; } = string.Empty;
}

// ── Widget (Apple/Google Pay) Request ───────────────────────────
public class WidgetRequest
{
    public decimal Amount { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

// ── Wallet Payment Request ───────────────────────────────────────
public class WalletPaymentRequest
{
    public string WalletId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "AZN";
    public string OrderId { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Language { get; set; } = "az";
}

// ── Invoice Create/Update Request ───────────────────────────────
public class InvoiceCreateRequest
{
    public decimal Sum { get; set; }
    public int Display { get; set; }
    public int SaveAsTemplate { get; set; }
    public int? StatusInstallment { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Inn { get; set; }
    public string? ContractNumber { get; set; }
    public string? MerchantOrderId { get; set; }
    public string PeriodFrom { get; set; } = string.Empty;
    public string PeriodTo { get; set; } = string.Empty;
}

public class InvoiceUpdateRequest : InvoiceCreateRequest
{
    public int Id { get; set; }
}

// ── Generic Epoint API Response ──────────────────────────────────
public class EpointApiResponse
{
    public string? Status { get; set; }
    public string? RedirectUrl { get; set; }
    public string? Transaction { get; set; }
    public string? CardId { get; set; }
    public string? Code { get; set; }
    public string? Message { get; set; }
    public string? BankTransaction { get; set; }
    public string? BankResponse { get; set; }
    public string? OperationCode { get; set; }
    public string? Rrn { get; set; }
    public string? CardName { get; set; }
    public string? CardMask { get; set; }
    public decimal? Amount { get; set; }
    public decimal? SplitAmount { get; set; }
    public string? OrderId { get; set; }
    public string? WidgetUrl { get; set; }
    public object? OtherAttr { get; set; }
    public string? Error { get; set; }
}
