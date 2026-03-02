using MediatR;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;

namespace SIWES360.Application.Features.Authentication.Commands.LoginUser
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<Result<TokenResponse>>;

    public sealed class LoginCommandHandler(IAuthenticationService _auth) : IRequestHandler<LoginCommand, Result<TokenResponse>>
    {
        public Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken ct) =>
       _auth.LoginAsync(request.Email, request.Password, ct);
    }
}