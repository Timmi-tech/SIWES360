namespace SIWES360.Infrastructure.Security
{
    public sealed class JwtConfiguration
{
    public const string Section = "JwtSettings";

    public string Secret { get; init; } = string.Empty;
    public string ValidIssuer { get; init; } = string.Empty;
    public string ValidAudience { get; init; } = string.Empty;
    public int ExpiresMinutes { get; init; } = 30;
}
}