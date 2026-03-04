using FluentValidation;

namespace SIWES360.Application.Features.Authentication.Commands.SetInvitedSupervisorPassword
{

    public class SetInvitedSupervisorPasswordCommandValidator : AbstractValidator<SetInvitedSupervisorPasswordCommand>
    {
        public SetInvitedSupervisorPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
