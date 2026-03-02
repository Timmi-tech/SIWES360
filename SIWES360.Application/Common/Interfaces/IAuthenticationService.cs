using SIWES360.Application.Common.Models;
using SIWES360.Application.Features.Authentication;
using SIWES360.Domain.Enums;

namespace SIWES360.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result> RegisterUserAsync(
            string firstname,
            string lastname,
            string MatricNo,
            string email,
            string password,
            UserRole role,
            Guid departmentId,
            CancellationToken ct);

        Task<Result<TokenResponse>> LoginAsync(
            string identifier,
            string password,
            CancellationToken ct);

        Task<Result<TokenResponse>> RefreshTokenAsync(
           string accessToken,
           string refreshToken,
           CancellationToken ct);
        Task<Result> RevokeAsync(
            string userId, 
            CancellationToken ct);

    }
}