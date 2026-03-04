using SIWES360.Application.Common.Models;

namespace SIWES360.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<Result> SendInviteSupervisorEmailAsync(
            string email,
            string fullName,
            string invitationToken,
            CancellationToken ct);

    }
}