using MediatR;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;
using SIWES360.Domain.Enums;

namespace SIWES360.Application.Features.Authentication.Commands
{
    public sealed record RegisterUserCommand(string Firstname, string Lastname, string Email, string Password, string MatricNo, UserRole Role, Guid DepartmentId) : IRequest<Result>;
    public sealed class RegisterUserCommandHandler(IAuthenticationService _auth) : IRequestHandler<RegisterUserCommand, Result>
    {
        public Task<Result> Handle(RegisterUserCommand request, CancellationToken ct) =>
       _auth.RegisterUserAsync(
           request.Firstname,
           request.Lastname,
           request.Email,
           request.Password,
           request.MatricNo,
           request.Role,
           request.DepartmentId,
           ct);
    }
}