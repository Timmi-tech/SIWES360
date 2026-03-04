using MediatR;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;

namespace SIWES360.Application.Features.Authentication.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<Result<TokenResponse>>;

public sealed class RefreshTokenCommandHandler(IAuthenticationService authService) : IRequestHandler<RefreshTokenCommand, Result<TokenResponse>>
{
    private readonly IAuthenticationService _authService = authService;

    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
        => await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken, ct);
}
