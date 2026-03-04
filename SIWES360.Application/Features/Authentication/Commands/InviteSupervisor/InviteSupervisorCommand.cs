using MediatR;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;

namespace SIWES360.Application.Features.Authentication.Commands.InviteSupervisor
{
    public sealed record InviteSupervisorCommand(string Email, string FullName, Guid DepartmentId) : IRequest<Result>;

    public sealed class InviteSupervisorCommandHandler(IAuthenticationService _auth) : IRequestHandler<InviteSupervisorCommand, Result>
    {
        public async Task<Result> Handle(InviteSupervisorCommand request, CancellationToken ct)
        {
            return await _auth.InviteSupervisorAsync(request.Email, request.FullName, request.DepartmentId, ct);
        }
    }

}