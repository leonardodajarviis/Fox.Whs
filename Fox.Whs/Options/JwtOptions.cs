namespace Fox.Whs.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string AccessSecret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessExpiryInMinutes { get; set; } = 60;
    public int RefreshExpiryInDays { get; set; } = 7;
}
