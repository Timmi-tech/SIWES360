using MediatR;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Application.Common.Models;

namespace SIWES360.Application.Features.Authentication.Commands.SetInvitedSupervisorPassword
{

    public record SetInvitedSupervisorPasswordCommand(string UserId, string Token, string NewPassword) : IRequest<Result>;

    public class SetInvitedSupervisorPasswordCommandHandler(IAuthenticationService _auth) : IRequestHandler<SetInvitedSupervisorPasswordCommand, Result>
    {
        public async Task<Result> Handle(SetInvitedSupervisorPasswordCommand request, CancellationToken ct)
        {
            return await _auth.SetInvitedSupervisorPasswordAsync(request.UserId, request.Token, request.NewPassword, ct);
        }
    }


}
