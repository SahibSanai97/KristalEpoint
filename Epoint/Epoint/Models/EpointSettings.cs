namespace Epoint.Models;

public class EpointSettings
{
    public string PublicKey { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://epoint.az/api/1";
}
