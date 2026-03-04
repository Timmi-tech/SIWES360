using FluentValidation;

namespace SIWES360.Application.Features.Authentication.Commands.InviteSupervisor;

public class InviteSupervisorCommandValidator : AbstractValidator<InviteSupervisorCommand>
{
    public InviteSupervisorCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DepartmentId)
            .NotEmpty();
    }
}
