using MediatR;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;

namespace SIWES360.Application.Features.Authentication.Commands.RevokeToken;

public sealed record RevokeTokenCommand : IRequest<Result>;

public sealed class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result>
{
    private readonly IAuthenticationService _authService;
    private readonly ICurrentUserService _currentUser;

    public RevokeTokenCommandHandler(IAuthenticationService authService, ICurrentUserService currentUser)
    {
        _authService = authService;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(_currentUser.UserId))
            return Result.Failure(Error.Validation("Unauthorized", "User not authenticated"));

        return await _authService.RevokeAsync(_currentUser.UserId, ct);
    }
}
