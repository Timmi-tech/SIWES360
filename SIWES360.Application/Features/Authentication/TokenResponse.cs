namespace SIWES360.Application.Features.Authentication
{
    public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken
);
}