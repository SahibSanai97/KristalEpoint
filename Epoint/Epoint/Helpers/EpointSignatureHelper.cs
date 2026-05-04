using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Epoint.Helpers;

public static class EpointSignatureHelper
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    /// <summary>
    /// Encodes an object to base64(json) — the "data" parameter.
    /// </summary>
    public static string BuildData(object payload)
    {
        string json = JsonSerializer.Serialize(payload, _jsonOptions);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    /// <summary>
    /// Builds the signature: base64( sha1(privateKey + data + privateKey) )
    /// The raw (binary) SHA1 output is base64-encoded, matching PHP's sha1($str, true).
    /// </summary>
    public static string BuildSignature(string privateKey, string data)
    {
        string signString = privateKey + data + privateKey;
        byte[] hashBytes = SHA1.HashData(Encoding.UTF8.GetBytes(signString));
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifies a callback signature from Epoint server.
    /// </summary>
    public static bool VerifySignature(string privateKey, string data, string receivedSignature)
    {
        string expected = BuildSignature(privateKey, data);
        return expected == receivedSignature;
    }

    /// <summary>
    /// Decodes a "data" field received in a callback.
    /// </summary>
    public static T? DecodeData<T>(string data)
    {
        string json = Encoding.UTF8.GetString(Convert.FromBase64String(data));
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }
}
