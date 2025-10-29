namespace Fox.Whs.Options;

public class SapServiceLayerOptions
{
    public const string SectionName = "SapServiceLayer";

    public string BaseUrl { get; set; } = string.Empty;
    public string CompanyDB { get; set; } = string.Empty;
    public int SessionTimeoutMinutes { get; set; } = 30;
    public int HttpTimeoutSeconds { get; set; } = 60;
}
