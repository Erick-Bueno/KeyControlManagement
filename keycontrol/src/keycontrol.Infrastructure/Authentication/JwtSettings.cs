namespace keycontrol.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string? Secretkey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiresInAccessToken { get; set; }
    public int ExpiresInRefreshToken { get; set; }
}
